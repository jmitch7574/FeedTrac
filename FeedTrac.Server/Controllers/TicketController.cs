using FeedTrac.Server.Database;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    public TicketController(ApplicationDbContext context, UserService userService, UserManager<ApplicationUser> userManager, ModuleService moduleService)
    {
        _context = context;
        _userService = userService;
        _userManager = userManager;
        _moduleService = moduleService;
    }
}
