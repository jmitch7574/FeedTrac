using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server.Controllers
{
    /// <summary>
    /// Controller for image endpoints
    /// </summary>
    [ApiController]
    [Route("image")]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FeedTracUserManager _userManager;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public ImageController(ApplicationDbContext context, FeedTracUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Endpoint for getting image file
        /// </summary>
        /// <param name="imageId">The id of the image</param>
        [HttpGet]
        [Route("{imageId}")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetImage(int imageId)
        {
            var user = await _userManager.RequireUser();
            
            // Simulate fetching the image from a database or storage
            MessageImage? image = await _context.Images
                .Include(i => i.Message)
                    .ThenInclude(m => m.Ticket)
                    .ThenInclude(t => t.Module)
                    .ThenInclude(m => m.StudentModule)
                    .ThenInclude(u => u.User)
                .Include(i => i.Message)
                    .ThenInclude(m => m.Ticket)
                    .ThenInclude(t => t.Module)
                    .ThenInclude(m => m.TeacherModule)
                    .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null)
            {
                return NotFound();
            }

            if (image.Message.Ticket.Module.StudentModule.Find(sm => sm.User.Id == user.Id) == null && image.Message.Ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
            {
                return Forbid("User does not have access to the ticket");
            }

            return File(image.ImageData, image.ImageType, image.ImageName);
        }
    }
}
