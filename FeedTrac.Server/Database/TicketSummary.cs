using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedTrac.Server.Database
{
    /// <summary>
    /// A class containing information about ticket summary for caching purposes
    /// </summary>
    public class TicketSummary
    {
        /// <summary>
        /// The unique ID of the summary
        /// </summary>
        [Required]
        [Key]
        public int SummaryId { get; set; }

        /// <summary>
        /// The text summary
        /// </summary>
        [Required]
        [Column(TypeName = "text")]
        public required string Content { get; set; }
        
        /// <summary>
        /// The message count of the ticket when this summary was made
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public required int MessageCount { get; set; }

        /// <summary>
        /// The ID of the ticket this summary belongs to
        /// </summary>
        [Column(TypeName ="int")]
        [Required]
        public required int TicketId { get; set; }

        /// <summary>
        /// A typed reference to the ticket this summary belongs to
        /// </summary>
        [ForeignKey(nameof(TicketId))]
        public FeedbackTicket Ticket { get; set; }

    }
}
