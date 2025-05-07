using System.Text;
using System.Text.Json;
using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using FeedTrac.Server.Models.Forms.Ticket;
using FeedTrac.Server.Models.Gemini;
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
    private readonly EmailService _emailService;
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>
    /// Constructor for Ticket Controller
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    /// <param name="moduleService"></param>
    /// <param name="imageService"></param>
    /// <param name="emailService"></param>
    /// <param name="clientFactory"></param>
    public TicketController(ApplicationDbContext context, FeedTracUserManager userManager, ModuleService moduleService, ImageService imageService, EmailService emailService,  IHttpClientFactory clientFactory)
    {
        _context = context;
        _userManager = userManager;
        _moduleService = moduleService;
        _imageService = imageService;
        _emailService = emailService;
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// Gets the tickets for the current user. If the user is a student, it returns the tickets they have made. If the user is a teacher, it returns the tickets for all modules they are assigned to.
    /// </summary>
    /// <response code="200">Returns user's tickets</response>
    /// <response code="401">User does not have access to ticket system</response>
    [HttpGet]
    [ProducesResponseType(typeof(TicketCollectionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyTickets()
    {
        ApplicationUser user = await _userManager.RequireUser();
        var allTickets = _context.Tickets
            .Include(t => t.Module)
            .ThenInclude(m => m.TeacherModule)
            .Include(t => t.Owner)
            .Include(t => t.Messages)
            .ThenInclude(m => m.Images)
            .Include(t => t.Messages)
            .ThenInclude(m => m.Author);
        
        if (User.IsInRole("Student"))
        {
            var tickets = allTickets.Where(tick => tick.Owner == user);
            return Ok(tickets.Select(t => new TicketResponseDto(t)));
        }

        if (User.IsInRole("Teacher"))
        {
            var modules = await _moduleService.GetUserModulesAsync();
            var tickets = new List<FeedbackTicket>();
            foreach (var module in modules)
            {
                var moduleTickets = allTickets.Where(tick => tick.Module == module).ToList();
                tickets.AddRange(moduleTickets);
            }
            return Ok(tickets.Select(t => new TicketResponseDto(t)));
        }

        return Unauthorized(new { error = "Admins do not have tickets"});
    }

    /// <summary>
    /// Gets a user's assigned tickets within a specific module
    /// </summary>
    /// <param name="moduleId">The module</param>
    /// <returns></returns>
    [HttpGet("getByModuleId/{moduleId}")]
    public async Task<IActionResult> GetMyTicketsInModule(int moduleId)
    {
        ApplicationUser user = await _userManager.RequireUser();
        var allTickets = _context.Tickets
            .Include(t => t.Module)
            .ThenInclude(m => m.TeacherModule)
            .Include(t => t.Owner)
            .Include(t => t.Messages)
            .ThenInclude(m => m.Images)
            .Include(t => t.Messages)
            .ThenInclude(m => m.Author)
            .Where(t => t.ModuleId == moduleId);
        
        if (User.IsInRole("Student"))
        {
            var tickets = allTickets.Where(tick => tick.Owner == user);
            return Ok(tickets.Select(t => new TicketResponseDto(t)));
        }

        if (User.IsInRole("Teacher"))
        {
            var modules = await _moduleService.GetUserModulesAsync();
            var tickets = new List<FeedbackTicket>();
            foreach (var module in modules)
            {
                var moduleTickets = allTickets.Where(tick => tick.Module == module).ToList();
                tickets.AddRange(moduleTickets);
            }
            return Ok(tickets.Select(t => new TicketResponseDto(t)));
        }

        return Unauthorized(new { error = "Admins do not have tickets"});
    }

    /// <summary>
    /// Get Specific Ticket Information
    /// </summary>
    /// <param name="id">ID of the ticket</param>
    /// <response code="200">Successfully returns ticket information</response>
    /// <response code="404">The ticket could not be found</response>
    /// <response code="401">The user does not have access to this ticket</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TicketResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTicket(int id)
    {
        var user = await _userManager.RequireUser();
        
        var ticket = await _context.Tickets
                .Include(t => t.Module)
                .ThenInclude(m => m.TeacherModule)
                .Include(t => t.Owner)
                .Include(t => t.Messages)
                .ThenInclude(m => m.Images)
                .Include(t => t.Messages)
                .ThenInclude(m => m.Author)
            .FirstOrDefaultAsync(t => t.TicketId == id);

        if (ticket == null)
            throw new ResourceNotFoundException();

        if (!ticket.DoesUserHaveAccess(user.Id))
            throw new UnauthorizedResourceAccessException();

        return Ok(new TicketResponseDto(ticket));
    }

    /// <summary>
    /// Endpoint for creating a ticket
    /// </summary>
    /// <param name="request">Ticket info, including title and an optional first message</param>
    /// <param name="moduleId">The id of the module this ticket should belong to</param>
    /// <response code ="200">Ticket created successfully</response>
    /// <response code="401">User does not have access to referenced module</response> 
    /// <response code ="404">Could not find referenced module</response>
    [HttpPost("{moduleId}/create")]
    [ProducesResponseType(typeof(TicketResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateTicket([FromForm] TicketCreateRequest request, int moduleId)
    {
        var user = await _userManager.RequireUser();
        
        var targetModule = await _context.Modules
            .Include(m => m.StudentModule)
                .ThenInclude(sm => sm.User)
            .Include(m => m.TeacherModule)
            .   ThenInclude(tm => tm.User)
            .FirstOrDefaultAsync(m => m.Id == moduleId);

        if (targetModule == null)
            throw new ResourceNotFoundException();


        if (targetModule.StudentModule.Find(sm => sm.User.Id == user.Id && sm.Module == targetModule) == null)
            throw new UnauthorizedResourceAccessException();
        
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

        await _context.SaveChangesAsync();

        await _emailService.NotifyTeachersAboutTicket(ticket);

        return Ok(new TicketResponseDto(ticket));
    }


    /// <summary>
    /// Adds a message update to a ticket
    /// </summary>
    /// <param name="request">The details of the message</param>
    /// <param name="ticketId">The ticket to append this message to</param>
    /// <response code="200">Message added successfully</response>
    /// <response code="404">Ticket not found</response>
    /// <response code="401">User does not have access to referenced ticket</response>
    [HttpPost("{ticketId}/addMessage")]
    [ProducesResponseType(typeof(TicketResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddMessageToTicket([FromForm] MessageCreateRequest request, int ticketId)
    {
        var user = await _userManager.RequireUser();
        
        var ticket = await _context.Tickets
            .Include(t => t.Owner)
            .Include(t => t.Module)
            .Include(t => t.Module)
                .ThenInclude(m => m.TeacherModule)
                    .ThenInclude(tm => tm.User)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);

        if (ticket == null)
            throw new  ResourceNotFoundException();

        if (ticket.Owner != user && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
            throw new UnauthorizedResourceAccessException();

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
        
        await _emailService.NotifyTicketMessage(message);
        return Ok(new TicketResponseDto(ticket));
    }

    /// <summary>
    /// Mark a ticket as resolved
    /// </summary>
    /// <param name="ticketId">Which ticket to mark as resolved</param>
    /// <response code="200">Ticket marked as resolved successfully</response>
    /// <response code="404">Referenced ticket could not be found</response>
    /// <response code="401">User does not have access to referenced ticket</response>
    [HttpPost("{ticketId}/markAsResolved")]
    [ProducesResponseType(typeof(TicketResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
            throw new ResourceNotFoundException();


        if (ticket.Owner != user && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
            throw new UnauthorizedResourceAccessException();

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

        await _emailService.TicketResolved(ticket);
        
        return Ok(new TicketResponseDto(ticket));
    }

    /// <summary>
    /// Generates an AI summary of a ticket
    /// </summary>
    /// <param name="ticketId"></param>
    /// <returns></returns>
    [HttpGet("{ticketId}/summarize")]
    public async Task<IActionResult> Summarize(int ticketId)
    {
        var user = await _userManager.RequireUser();
        List<string> messages = new() { """
                                        You are a summarization agent for a ticketing system within an academic institution.
                                        A "Ticket" in this scenario is a conversation between a student and the module leaders.
                                        Please summarize the following ticket in just two-to-three sentences, only replying with the summarization. 
                                        Give advice where possible (only if accurate), and be polite and formal, do not offend anyone. \n
                                        """};
                                        
        
        string? apiKey = Environment.GetEnvironmentVariable("GEMINI_KEY");
        var client = _clientFactory.CreateClient();
        
        if (apiKey == null)
            return Unauthorized(new { error = "The Gemini API Key is not setup"});
        
        FeedbackTicket? ticket = await _context.Tickets
            .Include(t => t.Messages)
            .ThenInclude(m => m.Author)
            .Include(t => t.Module)
            .ThenInclude(m => m.TeacherModule)
            .ThenInclude(tm => tm.User)
            .Include(feedbackTicket => feedbackTicket.Owner)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        
        if (ticket is null)
            throw new ResourceNotFoundException();

        if (ticket.Owner != user && ticket.Module.TeacherModule.Find(tm => tm.User.Id == user.Id) == null)
            throw new UnauthorizedResourceAccessException();

        if (ticket.Messages.Count == 0)
        {
            return Ok(new SummarisationResponse("A summary could not be made without any messages"));
        }

        messages.Add($"Title: {ticket.Title}");
        messages.Add($"Module: {ticket.Module.Name}");
        
        foreach (var message in ticket.Messages)
        {
            messages.Add($"{message.Author.FirstName} {message.Author.LastName}: {message.Content}");
        }
        
        GeminiRequest rq =  new GeminiRequest();
        GeminiContent cq = new GeminiContent();
        GeminiPart gp = new GeminiPart();
        gp.Text = string.Join("\n", messages);
        cq.Parts = new List<GeminiPart>() { gp };
        rq.Contents = new  List<GeminiContent>() { cq };
        
        
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";
        var json = JsonSerializer.Serialize(rq);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(url, content);
        var responseBody =  await response.Content.ReadAsStringAsync();
        
        
        if (!response.IsSuccessStatusCode)
            return BadRequest(new { error = $"The Gemini API returned a non-successful response {responseBody}" });
        
        using var doc = JsonDocument.Parse(responseBody);
        
        string text = doc
            .RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .ToString();
        
        return Ok(new SummarisationResponse(text));
    }
}
