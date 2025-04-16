namespace FeedTrac.Server.Models.Responses.Ticket
{
    public class TicketResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<MessageResponseDto> Messages { get; set; } = new List<MessageResponseDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsResolved { get; set; }
        public bool IsDeleted { get; set; }
        public int ImageId { get; set; }
    }
}
