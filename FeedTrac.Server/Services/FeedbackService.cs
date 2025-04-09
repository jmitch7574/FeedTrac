using FeedTrac.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server.Services;

public class FeedbackService
{

    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly ModuleService _moduleService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FeedbackService(ApplicationDbContext context, UserService userService, ModuleService moduleService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userService = userService;
        _moduleService = moduleService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<FeedbackTicket?> GetFeedbackByIdAsync(int feedbackId)
    {
        return await _context.FeedbackTicket.FirstOrDefaultAsync(f => f.TicketId == feedbackId);
    }

    public async Task<List<FeedbackTicket>> GetFeedbackForModuleAsync(int moduleId)
    {
        return await _context.FeedbackTicket.Where(f => f.ModuleId == moduleId).ToListAsync();
    }

    public async Task<FeedbackTicket> CreateFeedbackAsync(int moduleId, string title)
    {
        var userId = _userService.GetCurrentUserId();
        var module = await _moduleService.GetModuleAsync(moduleId);
        if (module == null)
            throw new Exception("Module not found.");

        var newFeedback = new FeedbackTicket
        {
            ModuleId = moduleId,
            OwnerId = userId,
            Title = title
        };
        _context.FeedbackTicket.Add(newFeedback);
        await _context.SaveChangesAsync();
        return newFeedback;
    }

    /// <summary>
    /// Gets all feedback items this user should be able to view
    /// For role 2 users, get the feedback they are an owner of
    /// For role 0 or 1 users, get all feedback for modules they are in
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<FeedbackTicket>> GetUserFeedBack()
    {
        ApplicationUser currentUser = await _userService.GetCurrentUserAsync();
        if (currentUser == null)
            throw new Exception("User not found.");

        // This LINQ statement should get feedback items that either
        // Belong to the currently signed in uesr
        // Belong to a module that the user is at least an instructor in (role 1)
        //List<FeedbackTicket> viewableTickets = await _context.FeedbackTicket.Where(f => f.OwnerId == currentUser.Id || f.Module.StudentModule.Where(u => u.Role <= 1).Any()).ToListAsync();

        return null;// viewableTickets;
    }
}
