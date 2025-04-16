using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses.Identity
{
    /// <summary>
    /// Authentication summary for the client. Can be used for any middleware
    /// </summary>
    public class AuthSummary
    {
        /// <summary>
        /// What level the user is currently authenticated at
        /// </summary>
        public enum AuthStatus
        {
            NotAuthenticated,
            AuthenticatedStudent,
            AuthenticatedTeacher,
            AuthenticatedAdmin
        }

        /// <summary>
        /// Auth status for client.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AuthStatus status { get; set; }

        /// <summary>
        /// Basic user info if the user is authenticated.
        /// </summary>
        public BaseUserInfo? userInfo { get; set; }


    }
}
