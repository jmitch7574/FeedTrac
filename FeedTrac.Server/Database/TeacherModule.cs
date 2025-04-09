namespace FeedTrac.Server.Database;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// A class that contains the relationship between a user and a module
/// </summary>
public class TeacherModule
{
    /// <summary>
    /// The unique identifier for this user-to-module relationship
    /// </summary>
    [Key]
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// The ID of the user in this relationship
    /// </summary>
    [Column(TypeName ="text")]
    [Required]
    public string UserId { get; set; }

    /// <summary>
    /// The typed reference to the user in this relationship
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    /// <summary>
    /// The ID of the module in this relationship
    /// </summary>
    [Column(TypeName = "integer")]
    [Required]
    public int ModuleId { get; set; }

    /// <summary>
    /// The typed reference to the user in this relationship
    /// </summary>
    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; }
}
