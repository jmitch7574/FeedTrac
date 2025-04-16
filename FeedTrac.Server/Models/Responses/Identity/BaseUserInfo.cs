using FeedTrac.Server.Database;

namespace FeedTrac.Server.Models.Responses.Identity
{
    /// <summary>
    /// Data structure containing basic user information.
    /// Prevents sending sensitive information to the client.
    /// </summary>
    public class BaseUserInfo
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Constructor for BaseUserInfo
        /// </summary>
        /// <param name="au">The application user to build the info from</param>
        public BaseUserInfo(ApplicationUser au)
        {
            UserId = au.Id;
            FirstName = au.FirstName ?? string.Empty;
            LastName = au.LastName ?? string.Empty;
            Email = au.Email ?? string.Empty;
        }

    }
}
