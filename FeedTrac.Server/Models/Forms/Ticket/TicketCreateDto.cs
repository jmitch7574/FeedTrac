namespace FeedTrac.Server.Models.Forms.Ticket
{
    public class TicketCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<int> ImageIds { get; set; } = new List<int>();
        public bool IsResolved { get; set; }
        public bool IsDeleted { get; set; }
        public int ImageId { get; set; }
    }
}
