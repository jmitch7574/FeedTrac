namespace FeedTrac.Server.Models.Responses.Ticket
{
    public class MessageResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<int> imageIds { get; set; } = new List<int>();
    }
}
