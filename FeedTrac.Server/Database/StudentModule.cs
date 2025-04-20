namespace FeedTrac.Server.Database;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// A class that contains the relationship between a user and a module
/// </summary>
public class StudentModule
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
    public required string UserId { get; set; }

    /// <summary>
    /// The typed reference to the user in this relationship
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public required ApplicationUser User { get; set; }

    /// <summary>
    /// The ID of the module in this relationship
    /// </summary>
    [Column(TypeName = "integer")]
    [Required]
    public required int ModuleId { get; set; }

    /// <summary>
    /// The typed reference to the user in this relationship
    /// </summary>
    [ForeignKey(nameof(ModuleId))]
    public required Module Module { get; set; }
}
