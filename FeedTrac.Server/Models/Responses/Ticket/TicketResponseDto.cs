using FeedTrac.Server.Database;

namespace FeedTrac.Server.Models.Responses.Ticket;

/// <summary>
/// Data transfer object for <see cref="FeedbackTicket"/> Object
/// </summary>
public class TicketResponseDto
{
    /// <summary>
    /// The Ticket's ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The Ticket's title
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// A list of messages made on the ticket
    /// </summary>
    public List<MessageResponseDto> Messages { get; set; }
    
    /// <summary>
    /// The timestamp where the ticket was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// The timestamp when the ticket was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Current ticket status
    /// </summary>
    public FeedbackTicket.TicketStatus Status { get; set; }
    
    /// <summary>
    /// Initializes the DTO from a ticket object
    /// </summary>
    /// <param name="ticket">The ticket to construct the DTO from</param>

    public TicketResponseDto(FeedbackTicket ticket)
    {
        Id = ticket.TicketId;
        Title = ticket.Title;
        Messages = ticket.Messages.Select(m => new MessageResponseDto(m)).ToList();
        CreatedAt = ticket.CreatedOn;
        UpdatedAt = ticket.LastUpdated;
        Status = ticket.Status;
    }
}

