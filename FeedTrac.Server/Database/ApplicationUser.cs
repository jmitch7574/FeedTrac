using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FeedTrac.Server.Database
{
    /// <summary>
    /// Contains information about our user accounts.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [ProtectedPersonalData]
        [Required(ErrorMessage = "First name is Requred")]
        [Column(TypeName = "varchar(255)")]
        public virtual string? FirstName { get; set; }

        [ProtectedPersonalData]
        [Column(TypeName = "varchar(255)")]
        [Required(ErrorMessage = "First name is Requred")]
        public virtual string? LastName { get; set; }

        [JsonIgnore]
        public virtual List<UserModule> UserModules { get; set; } = new();

        [JsonIgnore]
        public virtual List<FeedbackTicket> Tickets { get; set; } = new();
    }
}
