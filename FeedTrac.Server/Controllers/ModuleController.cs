using FeedTrac.Server.Database;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedTrac.Server.Controllers;

[ApiController]
[Route("modules")]
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

    [HttpGet]
    public async Task<IActionResult> GetModules()
    {
        try
        {
            var modules = await _moduleService.GetUserModulesAsync();
            return Ok(modules);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }


    [HttpPost]
    [Route("join")]
    public async Task<IActionResult> JoinModule(string joinCode)
    {
        if (_userService.GetCurrentUserId() == null)
            return Unauthorized("You must be signed in to join a module");

        try
        {
            var module = await _moduleService.JoinModule(joinCode);
            return Ok(module);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetModule(int id)
    {
        try
        {
            var module = await _moduleService.GetModuleAsync(id);
            return Ok(module);
        }
        catch
        {
            return NotFound("Module not found");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModule(int id)
    {
        try
        {
            await _moduleService.DeleteModuleAsync(id);
            return Ok("Module deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }   
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateModule(string name)
    {
        if (_userService.GetCurrentUserId() == null)
            return Unauthorized("You must be signed in to create a module");
    

        var module = await _moduleService.CreateModuleAsync(name);
        return Ok(module);
    }
}
