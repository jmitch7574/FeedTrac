using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedTrac.Server.Database
{
    /// <summary>
    /// A class containing information about a feedback ticket
    /// </summary>
    public class FeedbackTicket
    {
        public enum TicketStatus
        {
            Open,
            InProgress,
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
        public string OwnerId { get; set; }

        /// <summary>
        /// A typed reference to the owner of the ticket
        /// </summary>
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; set; }

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
        public Module Module { get; set; }

        /// <summary>
        /// The Feedback Title
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        public string Title { get; set; }

        /// <summary>
        /// The current status of the ticket
        /// </summary>
        public TicketStatus status { get; set; }

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
    }
}
