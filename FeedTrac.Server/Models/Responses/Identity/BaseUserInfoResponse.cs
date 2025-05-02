using FeedTrac.Server.Database;

namespace FeedTrac.Server.Models.Responses.Identity;

/// <summary>
/// Response type for use within endpoint <see cref="FeedTrac.Server.Controllers.IdentityController.IsAuthenticated"/>
/// </summary>
public class BaseUserInfoResponse
{
    /// <summary>
    /// The signed-in users ID
    /// </summary>
    public string UserId { get; set; } 
    
    /// <summary>
    /// The user's first name
    /// </summary>
    public string FirstName { get; set; }
    
    /// <summary>
    /// The user's last name
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// The user's email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Constructor for BaseUserInfo
    /// </summary>
    /// <param name="au">The application user to build the info from</param>
    public BaseUserInfoResponse(ApplicationUser au)
    {
        UserId = au.Id;
        FirstName = au.FirstName ?? string.Empty;
        LastName = au.LastName ?? string.Empty;
        Email = au.Email ?? string.Empty;
    }

}
