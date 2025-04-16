using FeedTrac.Server.Database;
using FeedTrac.Server.Models.Responses.Modules;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ModuleService _moduleService;

    public ModuleController(ApplicationDbContext context, UserService userService, UserManager<ApplicationUser> userManager, ModuleService moduleService)
    {
        _context = context;
        _userService = userService;
        _userManager = userManager;
        _moduleService = moduleService;
    }

    /// <summary>
    /// Retrieves a list of modules that the user is signed up to
    /// </summary>
    /// <example> GET /modules </example>
    /// <response code="200">Returns user's modules</response>
    /// <response code="404">The user is not signed in</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserModulesResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetUserModules()
    {
        try
        {
            var modules = await _moduleService.GetUserModulesAsync();
            return Ok(new UserModulesResponse(modules));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    /// <summary>
    /// Retrieves every module
    /// </summary>
    /// <example> GET /modules </example>
    /// <response code="200">Returns user's modules</response>
    /// <response code="404">The user does not have permissions</response>
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserModulesResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAllModules()
    {
        var modules = await _context.Modules.ToListAsync();
        return Ok(new UserModulesResponse(modules));
    }

    /// <summary>
    /// Joins signed in user to a module
    /// </summary>
    /// <param name="joinCode"></param>
    /// <example> POST modules/join?joinCode=xxxxxx </example>
    /// <response code="200">The user has been added</response>
    /// <response code="400">Failed to join the module</response>
    /// <response code="404">The user is not signed in</response>
    [HttpPost("join")]
    [Authorize(Roles = "Student")]
    [ProducesResponseType(typeof(ModuleResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> JoinModule(string joinCode)
    {
        if (_userService.GetCurrentUserId() == null)
            return Unauthorized("You must be signed in to join a module");

        try
        {
            var module = await _moduleService.JoinModule(joinCode);
            return Ok(new ModuleResponse(module));
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Assigns a Teacher to a module
    /// </summary>
    /// <param name="moduleId"></param>
    /// <param name="teacherId"></param>
    /// <response code="200">The teacher has been assigned</response>
    /// <response code="404">The module or teacher was not found</response>
    [HttpPost("/{moduleId}/assignTeacher/byTeacherId")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AssignTeacherById(string moduleId, string teacherId)
    {
        var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == int.Parse(moduleId));
        if (module == null)
            return NotFound("Module not found");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
        if (user == null)
            return NotFound("User not found");

        module.TeacherModule.Add(new TeacherModule
        {
            UserId = user.Id,
            ModuleId = module.Id
        });

        await _context.SaveChangesAsync();
        return Ok("Teacher assigned");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleId"></param>
    /// <param name="teacherEmail"></param>
    /// <returns></returns>
    [HttpPost("/{moduleId}/assignTeacher/byTeacherEmail")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AssignTeacherByEmail(string moduleId, string teacherEmail)
    {
        var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == int.Parse(moduleId));
        if (module == null)
            return NotFound("Module not found");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == teacherEmail);
        if (user == null)
            return NotFound("Teacher not found");

        List<string> userRoles = (await _userManager.GetRolesAsync(user)).ToList();
        if ((userRoles.Contains("Teacher") || userRoles.Contains("Admin")) == false)
            return BadRequest("User is not a teacher");

        if (module.TeacherModule.Find(sm => sm.UserId == user.Id && sm.Module == module) != null)
            throw new Exception("Teacher is already assigned to the module");


        module.TeacherModule.Add(new TeacherModule
        {
            UserId = user.Id,
            ModuleId = module.Id
        });

        await _context.SaveChangesAsync();
        return Ok("Teacher assigned");
    }


    /// <summary>
    /// Get module info
    /// </summary>
    /// <param name="id">The id of the module</param>
    /// <response code="200">Returns the module</response>
    /// <response code="404">The module was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ModuleResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetModule(int id)
    {
        try
        {
            var module = await _moduleService.GetModuleAsync(id);
            return Ok(new ModuleResponse(module));
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
    /// <response code="200">The module was deleted</response>
    /// <response code="404">The module was not found or the user does not have permission</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteModule(int id)
    {
        var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == id);
        if (module == null)
            return NotFound("Module not found");

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
    /// Student endpoint to remove a module
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">The user has left the module</response>
    /// <response code="404">The module was not found or the user does not have permission</response>
    [HttpDelete("{id}/leave")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Authorize]
    public async Task<IActionResult> LeaveModule(int id)
    {
        var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == id);

        if (module == null)
            return NotFound("Module not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // This is the Identity User ID
        if (module.StudentModule.FirstOrDefault(sm => sm.User == User.Identity) == null)
        {
            return NotFound("User not part of module");
        }

        module.StudentModule.Remove(module.StudentModule.FirstOrDefault(sm => sm.User == User.Identity));

        return Ok();
    }

    /// <summary>
    /// Create a module
    /// </summary>
    /// <param name="name"></param>
    /// <response code="200">The module was created</response>
    /// <response code="404">The user does not have permission</response>
    [HttpPost("/create")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ModuleResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateModule(string name)
    {
        if (_userService.GetCurrentUserId() == null)
            return Unauthorized("You must be signed in to create a module");

        var module = await _moduleService.CreateModuleAsync(name);
        return Ok(module);
    }
}
