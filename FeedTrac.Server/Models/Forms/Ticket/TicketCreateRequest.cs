namespace FeedTrac.Server.Models.Forms.Ticket;

/// <summary>
/// Request type for endpoint <see cref="FeedTrac.Server.Controllers.TicketController.CreateTicket"/>
/// </summary>
public class TicketCreateRequest
{
    /// <summary>
    /// Ticket Title
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional First message
    /// </summary>
    public MessageCreateRequest? FirstMessage { get; set; }
}
