
using FeedTrac.Server.Database;

namespace FeedTrac.Server.Services;

/// <summary>
/// Service used for getting Image data
/// </summary>
public class ImageService
{

    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the service
    /// </summary>
    /// <param name="context"></param>
    public ImageService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Upload an image as a byte array to the database
    /// </summary>
    /// <param name="image">The form file uploaded from the frontend</param>
    /// <param name="messageId">The id of the message the image is attached to</param>
    public async Task UploadImage(IFormFile image, int messageId)
    {
        using var ms = new MemoryStream();
        await image.CopyToAsync(ms);

        _context.Images.Add(new MessageImage
        {
            ImageData = ms.ToArray(),
            ImageName = image.FileName,
            ImageType = image.ContentType,
            MessageId = messageId
        });
    }

    
}

