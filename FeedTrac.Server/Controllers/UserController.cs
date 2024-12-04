namespace FeedTrac.Server.Controllers;

using FeedTrac.Server.Database;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("user")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly ModuleService _moduleService;

    public UserController(ApplicationDbContext context, UserService userService, ModuleService moduleService)
    {
        _context = context;
        _userService = userService;
        _moduleService = moduleService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var user = await _userService.GetCurrentUserAsync();
        if (user == null)
        {
            return NotFound("User not signed in");
        }

        return Ok(user);
    }

    [HttpGet]
    [Route("modules")]
    public async Task<IActionResult> GetModules() {
        var modules = await _moduleService.GetModulesAsync();
        return Ok(modules);
    }
}
