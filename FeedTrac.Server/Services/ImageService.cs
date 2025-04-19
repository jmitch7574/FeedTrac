
using FeedTrac.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server.Services;
public class ImageService
{

    private readonly ApplicationDbContext _context;

    public ImageService(ApplicationDbContext context)
    {
        _context = context;
    }

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

