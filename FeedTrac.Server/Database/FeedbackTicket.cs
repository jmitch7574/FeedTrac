using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedTrac.Server.Database
{
    /// <summary>
    /// A class containing information about a feedback ticket
    /// </summary>
    public class FeedbackTicket
    {
        /// <summary>
        /// Enum used to detail the status of a ticket
        /// </summary>
        public enum TicketStatus
        {
            /// <summary>
            /// Ticket is not closed and not yet received a message from a teacher
            /// </summary>
            Open,
            
            /// <summary>
            /// The ticket is not closed and has received at least one response from a teacher
            /// </summary>
            InProgress,
            
            /// <summary>
            /// Ticket has been marked complete by either student or teacher
            /// </summary>
            Closed,
        }

        /// <summary>
        /// The unique id of the ticket
        /// </summary>
        [Required]
        [Key]
        public int TicketId { get; set; }

        /// <summary>
        /// The ID of the owner of the ticket
        /// </summary>
        [Required]
        [Column(TypeName = "text")]
        public required string OwnerId { get; set; }

        /// <summary>
        /// A typed reference to the owner of the ticket
        /// </summary>
        [ForeignKey(nameof(OwnerId))]
        public required ApplicationUser Owner { get; set; }

        /// <summary>
        /// The ID of the owner of the ticket
        /// </summary>
        [Required]
        [Column(TypeName = "integer")]
        public int ModuleId { get; set; }

        /// <summary>
        /// The typed reference to the module this ticket belongs to
        /// </summary>
        [ForeignKey(nameof(ModuleId))]
        public required Module Module { get; set; }

        /// <summary>
        /// The Feedback Title
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        public required string Title { get; set; }

        /// <summary>
        /// The current status of the ticket
        /// </summary>
        public TicketStatus Status { get; set; }

        /// <summary>
        /// The timestamp the ticket was created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The last time the ticket was updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Messages within this ticket
        /// </summary>
        public List<FeedbackMessage> Messages { get; set; } = new();
        
        /// <summary>
        /// Summaries made for this ticket
        /// </summary>
        public List<TicketSummary> Summaries { get; set; } = new();

        /// <summary>
        /// Check if a user should have access to this ticket
        /// </summary>
        /// <param name="userId">The ID of the user to check</param>
        /// <returns>True if the user has access. False otherwise</returns>
        public bool DoesUserHaveAccess(string userId)
        {
            return OwnerId == userId || Module.TeacherModule.Any(x => x.UserId == userId);
        }
    }
}
