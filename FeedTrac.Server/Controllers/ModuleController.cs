using FeedTrac.Server.Database;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedTrac.Server.Controllers;

public class ModuleController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly ModuleService _moduleService;

    public ModuleController(ApplicationDbContext context, UserService userService, ModuleService moduleService)
    {
        _context = context;
        _userService = userService;
        _moduleService = moduleService;
    }

    public async Task<IActionResult> GetModules()
    {
        var modules = await _moduleService.GetUserModulesAsync();
        return Ok(modules);
    }

    public async Task<IActionResult> GetModule(int id)
    {
        var module = await _moduleService.GetModuleAsync(id);
        if (module == null)
        {
            return NotFound("Module not found");
        }

        return Ok(module);
    }
}
