namespace FeedTrac.Server.Models.Forms.Ticket;

/// <summary>
/// Request type for endpoint <see cref="FeedTrac.Server.Controllers.ModuleController.CreateModule"/>
/// </summary>
public class MessageCreateRequest
{
    /// <summary>
    /// Message Content
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Image files uploaded with the image
    /// </summary>
    public List<IFormFile> Images { get; } = new();
}

