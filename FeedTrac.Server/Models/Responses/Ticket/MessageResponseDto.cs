using FeedTrac.Server.Database;

namespace FeedTrac.Server.Models.Responses.Ticket;

/// <summary>
/// Data Transfer Object for a <see cref="FeedbackMessage"/> object
/// </summary>
public class MessageResponseDto
{
    /// <summary>
    /// The message ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// THe ID of the sender
    /// </summary>
    public string SenderId { get; set; }
    
    /// <summary>
    /// The name of the sender
    /// </summary>
    public string SenderName { get; set; }

    /// <summary>
    /// The message content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// The timestamp the message was created at 
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// A list of ids of the images attached to the message
    /// </summary>
    public List<int> ImageIds { get; set; }

    /// <summary>
    /// Constructor for the Data Transfer Object
    /// </summary>
    /// <param name="fm">The message to construct the information from</param>
    public MessageResponseDto(FeedbackMessage fm)
    {
        Id = fm.MessageId;
        SenderId = fm.AuthorId;
        SenderName = $"{fm.Author.FirstName} {fm.Author.LastName}";
        Content = fm.Content;
        CreatedAt = fm.CreatedAt;
        ImageIds = fm.Images.Select(i => i.Id).ToList();
    }
}
