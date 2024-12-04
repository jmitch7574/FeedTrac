using Microsoft.AspNetCore.Identity;
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
        [Column(TypeName = "varchar(255)")]
        public virtual string? FirstName { get; set; }

        [ProtectedPersonalData]
        [Column(TypeName = "varchar(255)")]
        public virtual string? LastName { get; set; }

        [JsonIgnore]
        public virtual List<UserModule> UserModules { get; set; } = new();
    }
}
