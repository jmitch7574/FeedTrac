using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using FeedTrac.Server.Models.Responses.Modules;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server.Controllers;

/// <summary>
/// A class containing the API endpoints for modules
/// </summary>
[ApiController]
[Route("modules")]
public class ModuleController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly FeedTracUserManager _userManager;
    private readonly ModuleService _moduleService;

    /// <summary>
    /// Constructor for Module Controller
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    /// <param name="moduleService"></param>
    public ModuleController(ApplicationDbContext context, FeedTracUserManager userManager, ModuleService moduleService)
    {
        _context = context;
        _userManager = userManager;
        _moduleService = moduleService;
    }

    /// <summary>
    /// Retrieves a list of modules that the user is signed up to
    /// </summary>
    /// <example> GET /modules </example>
    /// <response code="200">Returns user's modules</response>
    /// <response code="401">The user is not signed in</response>
    [HttpGet]
    [ProducesResponseType(typeof(ModuleCollectionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserModules()
    {
        List<Module> modules = await _moduleService.GetUserModulesAsync();
        return Ok(new ModuleCollectionDto(modules));
    }

    /// <summary>
    /// Retrieves every module
    /// </summary>
    /// <example> GET /modules </example>
    /// <response code="200">Returns user's modules</response>
    /// <response code="401">The user does not have permissions</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ModuleCollectionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllModules()
    {
        var modules = await _moduleService.GetAllModulesAsync();
        return Ok(new ModuleCollectionDto(modules));
    }

    /// <summary>
    /// Joins signed in user to a module
    /// </summary>
    /// <param name="joinCode"></param>
    /// <example> POST modules/join?joinCode=xxxxxx </example>
    /// <response code="200">The user has been added</response>
    /// <response code="400">Failed to join the module</response>
    /// <response code="401">The user is not signed in</response>
    [HttpPost("join")]
    [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> JoinModule(string joinCode)
    {
        var user = await _userManager.RequireUser("Student");

        var userId = user.Id;
        var module = await _context.Modules
            .Include(m => m.StudentModule)
            .Where(m => m.JoinCode == joinCode)
            .FirstOrDefaultAsync();

        if (module == null)
            throw new ResourceNotFoundException();

        if (module.StudentModule.Find(sm => sm.UserId == userId && sm.Module == module) != null)
            return BadRequest(new { error = "User is already a part of this module" });

        module.StudentModule.Add(new StudentModule
        {
            UserId = userId,
            User = user,
            ModuleId = module.Id,
            Module = module
        });
        
        await _context.SaveChangesAsync();
        return Ok(new ModuleDto(module));
    }

    /// <summary>
    /// Assigns a Teacher to a module
    /// </summary>
    /// <param name="moduleId"></param>
    /// <param name="teacherId"></param>
    /// <response code="200">The teacher has been assigned</response>
    /// <response code="404">The module or teacher was not found</response>
    /// <response code="401">The user is not authorized</response>
    /// <response code="400">The teacher could not be assigned</response>
    [HttpPost("/{moduleId}/assignTeacher/byTeacherId")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignTeacherById(string moduleId, string teacherId)
    {
        var user =  await _userManager.RequireUser("Admin");
        
        var module = await _context.Modules
            .Include(m => m.TeacherModule)
            .FirstOrDefaultAsync(m => m.Id == int.Parse(moduleId));

        if (module == null)
            throw new ResourceNotFoundException();
        
        var teacher = _context.Users.FirstOrDefault(u => u.Id == teacherId);
        
        if (teacher == null)
            throw new ResourceNotFoundException();
        
        if (!(await _userManager.IsInRoleAsync(teacher, "Teacher")))
            return BadRequest(new { error = "Given user is not a teacher" });

        if (module.TeacherModule.Find(sm => sm.UserId == user.Id && sm.Module == module) != null)
            return BadRequest(new { error = "Teacher is already assigned to the module" });


        module.TeacherModule.Add(new TeacherModule
        {
            UserId = user.Id,
            User = user,
            ModuleId = module.Id,
            Module = module
        });

        await _context.SaveChangesAsync();
        return Ok("Teacher assigned");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleId"></param>
    /// <param name="teacherEmail"></param>
    /// <response code="200">The teacher has been assigned</response>
    /// <response code="404">The module or teacher was not found</response>
    /// <response code="401">The user is not authorized</response>
    /// <response code="400">The teacher could not be assigned</response>
    [HttpPost("/{moduleId}/assignTeacher/byTeacherEmail")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignTeacherByEmail(string moduleId, string teacherEmail)
    {
        var user =  await _userManager.RequireUser("Admin");
        
        var module = await _context.Modules
            .Include(m => m.TeacherModule)
            .FirstOrDefaultAsync(m => m.Id == int.Parse(moduleId));

        if (module == null)
            throw new ResourceNotFoundException();
        
        var teacher = _context.Users.FirstOrDefault(u => u.Email == teacherEmail);
        
        if (teacher == null)
            throw new ResourceNotFoundException();
        
        if (!(await _userManager.IsInRoleAsync(teacher, "Teacher")))
            return BadRequest(new { error = "Given user is not a teacher" });

        if (module.TeacherModule.Find(sm => sm.UserId == user.Id && sm.Module == module) != null)
            return BadRequest(new { error = "Teacher is already assigned to the module" });


        module.TeacherModule.Add(new TeacherModule
        {
            UserId = user.Id,
            User = user,
            ModuleId = module.Id,
            Module = module
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
    /// <response code="400">The user is not authorized to access the requested module</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetModule(int id)
    {
        var user =  await _userManager.RequireUser();
        
        var module = await _context.Modules.Where(m => m.Id == id)
            .Include(m => m.StudentModule)
            .ThenInclude(sm => sm.User)
            .FirstOrDefaultAsync();

        if (module == null)
            throw new ResourceNotFoundException();

        if (! await _userManager.IsInRoleAsync(user, "Admin") && module.StudentModule.FirstOrDefault(sm => sm.UserId == user.Id) == null )
            throw new UnauthorizedAccessException();
        
        return Ok(new ModuleDto(module));
    }

    /// <summary>
    /// Delete a module
    /// </summary>
    /// <param name="id">The id of the module</param>
    /// <response code="200">The module was deleted</response>
    /// <response code="404">The module was not found or the user does not have permission</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteModule(int id)
    {
        await _userManager.RequireUser("Admin");
        
        Module? targetModule = await _context.Modules
            .FirstOrDefaultAsync(m => m.Id == id);

        if (targetModule == null)
            throw new ResourceNotFoundException();

        _context.Modules.Remove(targetModule);
        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Student endpoint to remove a module
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">The user has left the module</response>
    /// <response code="404">The module was not found</response>
    /// <response code="401">The user is not part of the module</response>
    [HttpDelete("{id}/leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> LeaveModule(int id)
    {
        var user = await _userManager.RequireUser("Student");
        
        var module = await _context.Modules
            .Include(m =>  m.StudentModule)
            .ThenInclude(sm => sm.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (module == null)
            throw new ResourceNotFoundException();

        StudentModule? sm = module.StudentModule.FirstOrDefault(sm => sm.User == user);

        if (sm == null)
            throw new UnauthorizedResourceAccessException();
        
        module.StudentModule.Remove(sm);
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Create a module
    /// </summary>
    /// <param name="name"></param>
    /// <response code="200">The module was created</response>
    /// <response code="401">The user is not authorized to create a module</response>
    [HttpPost("/create")]
    [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateModule(string name)
    {
        await _userManager.RequireUser("Admin");
        
        Module newModule = new Module
        {
            Name = name,
            JoinCode = Guid.NewGuid().ToString().Substring(0, 6)
        };

        _context.Modules.Add(newModule);
        await _context.SaveChangesAsync();
        return Ok(new ModuleDto(newModule));
    }
}
