using FeedTrac.Server.Database;

namespace FeedTrac.Server.Models.Responses.Ticket
{
    public class TicketResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<MessageResponseDto> Messages { get; set; } = new List<MessageResponseDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public FeedbackTicket.TicketStatus status { get; set; }

        public TicketResponseDto(FeedbackTicket ticket)
        {
            Id = ticket.TicketId;
            Title = ticket.Title;
            Messages = ticket.Messages.Select(m => new MessageResponseDto(m)).ToList();
            CreatedAt = ticket.CreatedOn;
            UpdatedAt = ticket.LastUpdated;
            status = ticket.status;
        }
    }
}
