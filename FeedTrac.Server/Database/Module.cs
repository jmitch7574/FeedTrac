namespace FeedTrac.Server.Database;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Module
{
    [Required]
    [Key]
    public virtual int Id { get; set; }

    [Column(TypeName = "char(6)")]
    [Required]
    public virtual string JoinCode { get; set; } = null!;


    [Column(TypeName = "varchar(255)")]
    [Required]
    public virtual string Name { get; set; } = null!;

    [JsonIgnore]
    public List<UserModule> UserModules { get; set; } = new();


}
