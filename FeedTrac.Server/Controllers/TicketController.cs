using FeedTrac.Server.Database;
using FeedTrac.Server.Models.Forms.Ticket;
using FeedTrac.Server.Models.Responses.Ticket;
using FeedTrac.Server.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace FeedTrac.Server.Controllers;

/// <summary>
/// A class containing the API endpoints for modules
/// </summary>
[ApiController]
[Route("tickets")]
public class TicketController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ModuleService _moduleService;
    private readonly ImageService _imageService;

    public TicketController(ApplicationDbContext context, UserService userService, UserManager<ApplicationUser> userManager, ModuleService moduleService, ImageService imageService)
    {
        _context = context;
        _userService = userService;
        _userManager = userManager;
        _moduleService = moduleService;
        _imageService = imageService;
    }

    /// <summary>
    /// Gets the tickets for the current user. If the user is a student, it returns the tickets they have made. If the user is a teacher, it returns the tickets for all modules they are assigned to.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetMyTickets()
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        if (User.IsInRole("Student"))
        {
            var tickets = _context.Tickets.Where(tick => tick.Owner == user).Include(t => t.Messages).ThenInclude(m => m.Images);
            return Ok(tickets.Select(t => new TicketResponseDto(t)));
        }

        if (User.IsInRole("Teacher"))
        {
            var modules = await _moduleService.GetUserModulesAsync();
            var tickets = new List<FeedbackTicket>();
            foreach (var module in modules)
            {
                var moduleTickets = _context.Tickets.Where(tick => tick.Module == module).Include(t => t.Messages).ToList();
                tickets.AddRange(moduleTickets);
            }
            return Ok(tickets.Select(t => new TicketResponseDto(t)));
        }

        return BadRequest("User is not a student or teacher");
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }
        var user = await _userService.GetCurrentUserAsync();

        if (ticket.Owner != await _userService.GetCurrentUserAsync() && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
        {
            return Forbid("User does not have access to the ticket");
        }

        return Ok(new TicketResponseDto(ticket));
    }

    [HttpPost("{moduleId}/create")]
    [Authorize]
    public async Task<IActionResult> CreateTicket([FromForm] TicketCreateDto request, int moduleId)
    {
        var targetModule = await _context.Modules.Include(m => m.StudentModule)
        .ThenInclude(sm => sm.User).FirstOrDefaultAsync(m => m.Id == moduleId);

        if (targetModule == null)
        {
            return NotFound("Module not found");
        }


        if (targetModule.StudentModule.Find(sm => sm.User.Id == _userService.GetCurrentUserId() && sm.Module == targetModule) == null)
        {
            return BadRequest("User is not a part of the module");
        }

        var user = await _userService.GetCurrentUserAsync();
        var ticket = new FeedbackTicket
        {
            Module = targetModule,
            Owner = user,
            Title = request.Title,
            CreatedOn = DateTime.UtcNow,
            status = FeedbackTicket.TicketStatus.Open,
            LastUpdated = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);

        if (request.FirstMessage != null)
        {
            var message = new FeedbackMessage
            {
                Ticket = ticket,
                TicketId = ticket.TicketId,
                Author = user,
                AuthorId = user.Id,
                Content = request.FirstMessage.Content,
                CreatedAt = DateTime.UtcNow
            };
            _context.Messages.Add(message);

            if (request.FirstMessage.Images != null)
            {
                foreach (var image in request.FirstMessage.Images)
                {
                    await _imageService.UploadImage(image, message.MessageId);
                }
            }
        }

        await _context.SaveChangesAsync();

        return Ok();
    }


    [HttpPost("{ticketId}/addMessage")]
    [Authorize]
    public async Task<IActionResult> AddMessageToTicket([FromForm] MessageCreateDto request, int ticketId)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Owner)
            .Include(t => t.Module)
                .ThenInclude(m => m.TeacherModule)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);

        if (ticket == null)
        {
            return NotFound("Ticket not found");
        }

        var user = await _userService.GetCurrentUserAsync();

        if (ticket.Owner != await _userService.GetCurrentUserAsync() && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
        {
            return Forbid("User does not have access to the ticket");
        }

        var message = new FeedbackMessage
        {
            Ticket = ticket,
            TicketId = ticket.TicketId,
            Author = user,
            AuthorId = user.Id,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        // Add update to the ticket
        ticket.LastUpdated = DateTime.UtcNow;

        // If the message is from a teacher, set the status to "In Progress"
        if (User.IsInRole("Teacher"))
        {
            ticket.status = FeedbackTicket.TicketStatus.InProgress;
        }

        // Update changes made to the ticket
        _context.Tickets.Update(ticket);

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        if (request.Images != null)
        {
            foreach (var image in request.Images)
            {
                await _imageService.UploadImage(image, message.MessageId);
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{ticketId}/markAsResolved")]
    public async Task<IActionResult> MarkAsResolved(int ticketId)
    {
        FeedbackTicket? ticket = await _context.Tickets
            .Include(t => t.Owner)
            .Include(t => t.Module)
                .ThenInclude(m => m.TeacherModule)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);


        var user = await _userService.GetCurrentUserAsync();

        if (ticket.Owner != await _userService.GetCurrentUserAsync() && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
        {
            return Forbid("User does not have access to the ticket");
        }

        ticket.status = FeedbackTicket.TicketStatus.Closed;
        ticket.LastUpdated = DateTime.UtcNow;
        ticket.Messages.Add(new FeedbackMessage
        {
            Ticket = ticket,
            TicketId = ticket.TicketId,
            Author = ticket.Owner,
            AuthorId = ticket.OwnerId,
            Content = "Ticket has been marked as resolved",
            CreatedAt = DateTime.UtcNow
        });
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();

        return Ok();
    }


}
