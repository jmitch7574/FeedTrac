using FeedTrac.Server.Database;

namespace FeedTrac.Server.Models.Responses.Ticket
{
    public class MessageResponseDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<int> imageIds { get; set; } = new List<int>();

        public MessageResponseDto(FeedbackMessage fm)
        {
            Id = fm.MessageId;
            SenderId = fm.AuthorId;
            SenderName = $"{fm.Author.FirstName} {fm.Author.LastName}";
            Content = fm.Content;
            CreatedAt = fm.CreatedAt;
            imageIds = fm.Images.Select(i => i.Id).ToList();
        }
    }
}
