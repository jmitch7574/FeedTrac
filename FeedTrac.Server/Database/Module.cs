namespace FeedTrac.Server.Database;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Module
{
    [Key]
    [Column(TypeName = "int")]
    public virtual int Id { get; set; }

    [Column(TypeName = "char(6)")]
    [Required]
    public virtual string JoinCode { get; set; } = null!;


    [Column(TypeName = "varchar(255)")]
    [Required]
    public virtual string Name { get; set; } = null!;

    public virtual List<ApplicationUser> Users { get; set; } = new();

}
