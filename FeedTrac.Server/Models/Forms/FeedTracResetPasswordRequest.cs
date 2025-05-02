namespace FeedTrac.Server.Models.Forms;

/// <summary>
/// Request Type for endpoint <see cref="FeedTrac.Server.Controllers.IdentityController.ResetPassword"/>
/// </summary>
public class FeedTracResetPasswordRequest
{
	/// <summary>
	/// The user's current password
	/// </summary>
	public required string CurrentPassword { get; set; }
	
	/// <summary>
	/// The password the user would like to change to
	/// </summary>
	public required string NewPassword { get; set; }
}