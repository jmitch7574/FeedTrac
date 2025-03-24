namespace FeedTrac.Server.Models.Responses.Identity
{
    /// <summary>
    /// The response given for the /identity/teacher/resgister endpoint
    /// </summary>
    public class RegisteredTeacher
    {
        /// <summary>
        /// The Two factor key of the created teacher
        /// </summary>
        public required string TwoFactorKey { get; set; }
    }
}
