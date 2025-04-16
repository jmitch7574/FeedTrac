namespace FeedTrac.Server.Models.Forms.Ticket
{
    public class MessageCreateDto
    {
        public string Content { get; set; } = string.Empty;
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public bool IsResolved { get; set; }
        public bool IsDeleted { get; set; }
        public int ImageId { get; set; }
    }
}
