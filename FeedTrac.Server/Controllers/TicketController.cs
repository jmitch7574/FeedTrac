using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using FeedTrac.Server.Models.Forms.Ticket;
using FeedTrac.Server.Models.Responses.Ticket;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server.Controllers;

/// <summary>
/// A class containing the API endpoints for modules
/// </summary>
[ApiController]
[Route("tickets")]
public class TicketController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly FeedTracUserManager _userManager;
    private readonly ModuleService _moduleService;
    private readonly ImageService _imageService;

    /// <summary>
    /// Constructor for Ticket Controller
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    /// <param name="moduleService"></param>
    /// <param name="imageService"></param>
    public TicketController(ApplicationDbContext context, FeedTracUserManager userManager, ModuleService moduleService, ImageService imageService)
    {
        _context = context;
        _userManager = userManager;
        _moduleService = moduleService;
        _imageService = imageService;
    }

    /// <summary>
    /// Gets the tickets for the current user. If the user is a student, it returns the tickets they have made. If the user is a teacher, it returns the tickets for all modules they are assigned to.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetMyTickets()
    {
        ApplicationUser user = await _userManager.RequireUser();
        
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

    /// <summary>
    /// Get Specific Ticket Information
    /// </summary>
    /// <param name="id">ID of the ticket</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TicketResponseDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetTicket(int id)
    {
        var user = await _userManager.RequireUser();
        
        var ticket = await _context.Tickets
            .Include(ticket => ticket.Owner)
            .Include(ticket => ticket.Module)
                .ThenInclude(module => module.TeacherModule)
            .FirstOrDefaultAsync(t => t.TicketId == id);

        if (ticket == null)
        {
            return NotFound();
        }

        if (!ticket.DoesUserHaveAccess(user.Id))
        {
            return Forbid("User does not have access to the ticket");
        }

        return Ok(new TicketResponseDto(ticket));
    }

    /// <summary>
    /// Endpoint for creating a ticket
    /// </summary>
    /// <param name="request">Ticket info, including title and an optional first message</param>
    /// <param name="moduleId">The id of the module this ticket should belong to</param>
    /// <returns></returns>
    [HttpPost("{moduleId}/create")]
    public async Task<IActionResult> CreateTicket([FromForm] TicketCreateRequest request, int moduleId)
    {
        var user = await _userManager.RequireUser();
        
        var targetModule = await _context.Modules
            .Include(m => m.StudentModule)
                .ThenInclude(sm => sm.User)
            .FirstOrDefaultAsync(m => m.Id == moduleId);

        if (targetModule == null)
        {
            return NotFound("Module not found");
        }


        if (targetModule.StudentModule.Find(sm => sm.User.Id == user.Id && sm.Module == targetModule) == null)
        {
            return BadRequest("User is not a part of the module");
        }
        
        var ticket = new FeedbackTicket
        {
            Module = targetModule,
            OwnerId = user.Id,
            Owner = user,
            Title = request.Title,
            CreatedOn = DateTime.UtcNow,
            Status = FeedbackTicket.TicketStatus.Open,
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

            foreach (var image in request.FirstMessage.Images)
            {
                await _imageService.UploadImage(image, message.MessageId);
            }
        }

        await _context.SaveChangesAsync();

        return Ok();
    }


    /// <summary>
    /// Adds a message update to a ticket
    /// </summary>
    /// <param name="request">The details of the message</param>
    /// <param name="ticketId">The ticket to append this message to</param>
    /// <returns></returns>
    [HttpPost("{ticketId}/addMessage")]
    public async Task<IActionResult> AddMessageToTicket([FromForm] MessageCreateRequest request, int ticketId)
    {
        var user = await _userManager.RequireUser();
        
        var ticket = await _context.Tickets
            .Include(t => t.Owner)
            .Include(t => t.Module)
                .ThenInclude(m => m.TeacherModule)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);

        if (ticket == null)
        {
            return NotFound("Ticket not found");
        }

        if (ticket.Owner != user && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
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
            ticket.Status = FeedbackTicket.TicketStatus.InProgress;
        }

        // Update changes made to the ticket
        _context.Tickets.Update(ticket);

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        foreach (var image in request.Images)
        {
            await _imageService.UploadImage(image, message.MessageId);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Mark a ticket as resolved
    /// </summary>
    /// <param name="ticketId">Which ticket to mark as resolved</param>
    /// <returns></returns>
    [HttpPost("{ticketId}/markAsResolved")]
    public async Task<IActionResult> MarkAsResolved(int ticketId)
    {
        var user = await _userManager.RequireUser();
        
        FeedbackTicket? ticket = await _context.Tickets
            .Include(t => t.Owner)
            .Include(t => t.Module)
                .ThenInclude(m => m.TeacherModule)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);

        if (ticket is null)
            return NotFound("Ticket could not be found");


        if (ticket.Owner != user && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
        {
            return Forbid("User does not have access to the ticket");
        }

        ticket.Status = FeedbackTicket.TicketStatus.Closed;
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
