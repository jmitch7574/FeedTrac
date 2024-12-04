namespace FeedTrac.Server.Database;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserModule
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Column(TypeName ="text")]
    [Required]
    public string UserId { get; set; }
    [Column(TypeName = "integer")]
    [Required]
    public int ModuleId { get; set; }
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }
    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; }

    public int Role { get; set; }

}
