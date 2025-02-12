namespace FeedTrac.Server.Database;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/// <summary>
/// A class containing information about a module
/// </summary>
public class Module
{
    /// <summary>
    /// The unique ID of the module
    /// </summary>
    [Required]
    [Key]
    public virtual int Id { get; set; }

    /// <summary>
    /// The module's join code
    /// </summary>
    [Column(TypeName = "char(6)")]
    [Required]
    public virtual string JoinCode { get; set; } = null!;

    /// <summary>
    /// The name of the module
    /// </summary>
    [Column(TypeName = "varchar(255)")]
    [Required]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Reference to UserModules relationship
    /// </summary>
    [JsonIgnore]
    public List<UserModule> UserModules { get; set; } = new();

    /// <summary>
    /// The list of tickets this module has
    /// </summary>
    [JsonIgnore]
    public List<FeedbackTicket> Tickets { get; set; } = new();


}
