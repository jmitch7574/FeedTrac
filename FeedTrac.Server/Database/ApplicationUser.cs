using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [Required(ErrorMessage = "First name is Requred")]
        [Column(TypeName = "varchar(255)")]
        public virtual string? FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [ProtectedPersonalData]
        [Column(TypeName = "varchar(255)")]
        [Required(ErrorMessage = "First name is Requred")]
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
    }
}
