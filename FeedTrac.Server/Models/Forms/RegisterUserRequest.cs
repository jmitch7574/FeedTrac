namespace FeedTrac.Server.Models.Forms;

/// <summary>
/// Request type for endpoint <see cref="FeedTrac.Server.Controllers.IdentityController.RegisterStudent"/>
/// </summary>
public class RegisterUserRequest
{
    /// <summary>
    /// The user's email address
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The user's password
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// The user's First Name
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// The user's Last Name
    /// </summary>
    public required string LastName { get; init; }
}
