using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedTrac.Server.Database
{
    public class FeedbackTicket
    {
        [Required]
        [Key]
        public int TicketId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; set; }

        [Required]
        [Column(TypeName = "integer")]
        public int ModuleId { get; set; }
        [ForeignKey(nameof(ModuleId))]
        public Module Module { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Title { get; set; }

    }
}
