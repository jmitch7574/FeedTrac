namespace FeedTrac.Server.Models.Forms.Ticket
{
    public class TicketCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public MessageCreateDto? FirstMessage { get; set; }
    }
}
