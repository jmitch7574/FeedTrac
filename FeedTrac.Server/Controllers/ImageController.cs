using FeedTrac.Server.Database;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace FeedTrac.Server.Controllers
{
    [ApiController]
    [Route("image")]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public ImageController(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Route("{imageId}")]
        public async Task<IActionResult> GetImage(int imageId)
        {
            // Simulate fetching the image from a database or storage
            MessageImage? image = await _context.Images
                .Include(i => i.Message)
                    .ThenInclude(m => m.Ticket)
                    .ThenInclude(t => t.Module)
                    .ThenInclude(m => m.StudentModule)
                .Include(i => i.Message)
                    .ThenInclude(m => m.Ticket)
                    .ThenInclude(t => t.Module)
                    .ThenInclude(m => m.TeacherModule)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
            {
                return NotFound();
            }

            var user = await _userService.GetCurrentUserAsync();

            if (image.Message.Ticket.Module.StudentModule.Find(sm => sm.User.Id == user.Id) == null && image.Message.Ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
            {
                return Forbid("User does not have access to the ticket");
            }

            return File(image.ImageData, image.ImageType, image.ImageName);
        }
    }
}
