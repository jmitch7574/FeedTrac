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
    /// The Many-to-Many relationships between Students and this module
    /// </summary>
    [JsonIgnore]
    public List<StudentModule> StudentModule { get; init; } = new();

    /// <summary>
    /// The Many-to-Many relationships between Teachers and this module
    /// </summary>
    [JsonIgnore]
    public List<TeacherModule> TeacherModule { get; init; } = new();

    /// <summary>
    /// The list of tickets this module has
    /// </summary>
    [JsonIgnore]
    public List<FeedbackTicket> Tickets { get; set; } = new();

    /// <summary>
    /// Checks if a user is part of this module
    /// </summary>
    /// <param name="userId">id of the user</param>
    /// <returns>True if the user is a part, false otherwise</returns>
    public bool IsUserPartOfModule(string userId)
    {
        return StudentModule.Any(x => x.UserId == userId) || TeacherModule.Any(x => x.UserId == userId);
    }


}
