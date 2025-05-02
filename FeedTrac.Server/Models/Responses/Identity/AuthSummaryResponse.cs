using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses.Identity;

/// <summary>
/// Authentication Response for endpoint <see cref="FeedTrac.Server.Controllers.IdentityController.IsAuthenticated"/>
/// </summary>
public class AuthSummaryResponse
{
    /// <summary>
    /// What level the user is currently authenticated at
    /// </summary>
    public enum AuthStatus
    {
        /// <summary>
        /// User is not signed in
        /// </summary>
        NotAuthenticated,
        
        /// <summary>
        /// User is signed in as a student
        /// </summary>
        AuthenticatedStudent,
        
        /// <summary>
        /// User is signed in as a teacher
        /// </summary>
        AuthenticatedTeacher,
        
        /// <summary>
        /// User is signed in as an Admin
        /// </summary>
        AuthenticatedAdmin
    }

    /// <summary>
    /// Auth status for client.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AuthStatus Status { get; set; }

    /// <summary>
    /// Basic user info if the user is authenticated.
    /// </summary>
    public BaseUserInfoResponse? UserInfo { get; set; }


}
