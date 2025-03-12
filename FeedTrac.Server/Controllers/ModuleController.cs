using FeedTrac.Server.Database;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedTrac.Server.Controllers;

/// <summary>
/// A class containing the API endpoints for modules
/// </summary>
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

    /// <summary>
    /// Retrieves a list of modules that the user is signed up to
    /// </summary>
    /// <example> GET /modules </example>
    /// <returns>
    ///     <b>200:</b> Returns the list of modules <br/>
    ///     <b>401:</b> The user is not signed in <br/>
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Module>), 200)]
    [ProducesResponseType(401)]
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

    /// <summary>
    /// API endpoint for joining the current user to a module
    /// </summary>
    /// <param name="joinCode"></param>
    /// <example> POST modules/join?joinCode=xxxxxx </example>
    /// <returns>
    ///     <b>200:</b> The user has been successfully joined to the module <br/>
    ///     <b>400:</b> Failed to join the module <br/>
    ///     <b>401:</b> The user is not signed in <br/>
    /// </returns>
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

    /// <summary>
    /// Get info about a module
    /// </summary>
    /// <param name="id">The id of the module</param>
    /// <example> GET modules/[moduleId] </example>
    /// <returns>
    ///     <b>200:</b> Returns the module <br/>
    ///     <b>404:</b> The module was not found <br/>
    /// </returns>
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

    /// <summary>
    /// Delete a module
    /// </summary>
    /// <param name="id">The id of the module</param>
    /// <returns>
    ///     <b>200:</b> The module has been deleted <br/>
    ///     <b>400:</b> Failed to delete the module <br/>
    /// </returns>
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

    /// <summary>
    /// Create a module
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateModule(string name)
    {
        if (_userService.GetCurrentUserId() == null)
            return Unauthorized("You must be signed in to create a module");
        // TODO: Implement lecturer role check
        //if (_userService.GetCurrentUserAsync().Result.Role != 0)
        //    return Unauthorized("You must be a lecturer to create a module");

        var module = await _moduleService.CreateModuleAsync(name);
        return Ok(module);
    }
}
