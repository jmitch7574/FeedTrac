using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OtpNet;


namespace FeedTrac.Server.Database
{
    /// <summary>
    /// Contains information about our user accounts
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's first name
        /// </summary>
        [ProtectedPersonalData]
        [Required(ErrorMessage = "First name is Required")]
        [Column(TypeName = "varchar(255)")]
        public virtual string? FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [ProtectedPersonalData]
        [Column(TypeName = "varchar(255)")]
        [Required(ErrorMessage = "Last name is Required")]
        public virtual string? LastName { get; set; }

        /// <summary>
        /// A list of modules that the user has access to
        /// </summary>
        [JsonIgnore]
        public virtual List<StudentModule> EnrolledModules { get; set; } = new();

        /// <summary>
        /// A list of modules that the user has access to
        /// </summary>
        [JsonIgnore]
        public virtual List<TeacherModule> TeachingModules { get; set; } = new();

        /// <summary>
        /// A list of tickets that the user has created
        /// </summary>
        [JsonIgnore]
        public virtual List<FeedbackTicket> Tickets { get; set; } = new();

        /// <summary>
        /// The Two factor secret used by 2FA apps to generate a Time-based One Time Password (TOTP)
        /// </summary>
        [JsonIgnore]
        [Column(TypeName = "char(32)")]
        public string TwoFactorSecret { get; set; } = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));

        /// <summary>
        /// This field only exists here so EF doesn't map it, may change in future
        /// </summary>
        // Convert to Base32 for Google Authenticator
        [NotMapped] // Prevents EF from persisting this field
        public override bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Takes a user-submitted TOTP and checks if it is correct
        /// </summary>
        /// <param name="token">a TOTP</param>
        /// <returns>True if correct, false otherwise</returns>
        public bool Confirm2FaToken(string token)
        {
            var otp = new Totp(Base32Encoding.ToBytes(this.TwoFactorSecret), step: 30, mode: OtpHashMode.Sha1);
            return otp.VerifyTotp(token, out _);
        }
    }
}

