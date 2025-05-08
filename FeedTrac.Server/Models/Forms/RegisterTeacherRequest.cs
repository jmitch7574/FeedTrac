namespace FeedTrac.Server.Models.Forms;

/// <summary>
/// Request type for endpoint <see cref="FeedTrac.Server.Controllers.IdentityController.RegisterTeacher"/>
/// </summary>
public class RegisterTeacherRequest
{
	/// <summary>
	/// The user's email address
	/// </summary>
	public required string Email { get; init; }

	/// <summary>
	/// The user's First Name
	/// </summary>
	public required string FirstName { get; init; }

	/// <summary>
	/// The user's Last Name
	/// </summary>
	public required string LastName { get; init; }
}