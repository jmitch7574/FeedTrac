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
    /// First message
    /// </summary>
    public required MessageCreateRequest FirstMessage { get; set; }
}
