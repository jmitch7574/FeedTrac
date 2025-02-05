using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedTrac.Server.Database
{
    public class FeedbackMessage
    {
        [Required]
        [Key]
        public int MessageId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public required string Content { get; set; }

        [Required]
        public required FeedbackTicket Ticket { get; set; }

        [Required]
        public required ApplicationUser Author { get; set; }

        [Required]
        public required DateTime CreatedAt { get; set; }
    }
}
