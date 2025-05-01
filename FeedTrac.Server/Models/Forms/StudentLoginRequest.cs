namespace FeedTrac.Server.Models.Forms;

/// <summary>
/// Request type for endpoint <see cref="FeedTrac.Server.Controllers.IdentityController.StudentLogin"/>
/// </summary>
public class StudentLoginRequest
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
    /// An optional parameter to indicate if the user's session should be remembered across browser restarts.
    /// </summary>
    public bool RememberMe { get; init; } = false;
}
