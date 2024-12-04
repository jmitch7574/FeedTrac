namespace FeedTrac.Server.Database;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Keyless]
public class JoinedModule
{
    [Required]
    [Column(TypeName = "int")]
    public virtual ApplicationUser user { get; set; } = new ApplicationUser();

    [Required]
    [Column(TypeName = "int")]
    public virtual Module module { get; set; } = new Module();

}
