using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedTrac.Server.Database
{
    /// <summary>
    /// A class containing information about a message on a ticket 
    /// </summary>
    public class FeedbackMessage
    {
        /// <summary>
        /// The unique ID of the message
        /// </summary>
        [Required]
        [Key]
        public int MessageId { get; set; }

        /// <summary>
        /// The text content of the message
        /// </summary>
        [Required]
        [Column(TypeName = "text")]
        public required string Content { get; set; }

        /// <summary>
        /// Referenced Images for the message
        /// </summary>
        public List<MessageImages> Images { get; set; }

        /// <summary>
        /// The ID of the ticket this message belongs to
        /// </summary>
        [Column(TypeName ="int")]
        [Required]
        public int TicketId { get; set; }

        /// <summary>
        /// A typed reference to the ticket this message belongs to
        /// </summary>
        [ForeignKey(nameof(TicketId))]
        public required FeedbackTicket Ticket { get; set; }

        /// <summary>
        /// Id of the author
        /// </summary>
        public required string AuthorId { get; set; }
        /// <summary>
        /// The author of the message
        /// </summary>
        [Required]
        public required ApplicationUser Author { get; set; }

        /// <summary>
        /// The timesetamp of when the message was created
        /// </summary>
        [Required]
        public required DateTime CreatedAt { get; set; }
    }
}
