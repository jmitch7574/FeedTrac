using FeedTrac.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server.Services;

/// <summary>
/// Service class for modules
/// </summary>
public class ModuleService
{
    private readonly ApplicationDbContext _context;
    private readonly FeedTracUserManager _userManager;

    /// <summary>
    /// Initializes the module service
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    public ModuleService(ApplicationDbContext context, FeedTracUserManager userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Returns all modules that a user has access to
    /// </summary>
    /// <returns></returns>
    public async Task<List<Module>> GetUserModulesAsync()
    {
        // Get our module
        var user = await _userManager.RequireUser();
        
        // Get all applicable modules
        List<Module> modules = await _context.Modules
            .Include(m => m.StudentModule)
            .ThenInclude(sm => sm.User)
            .Include(m => m.TeacherModule)
            .ThenInclude(tm => tm.User)
            .Where(
                m => m.StudentModule.FirstOrDefault(sm => sm.User == user) != null ||
                     m.TeacherModule.FirstOrDefault(tm => tm.User == user) != null
            ).ToListAsync();

        return await _context.Modules.Where(m => m.UserModules.Any(u => u.User.Id == userId)).ToListAsync();
    }

    public async Task<Module> GetModuleAsync(int id)
    {
        var module = await _context.Modules.Where(m => m.Id == id).Include(m => m.UserModules).FirstOrDefaultAsync();
        if (module == null)
            throw new Exception("Module not found.");

        return module;
    }

    public async Task<Module> JoinModule(string joinCode)
    {
        var userId = _userService.GetCurrentUserId();
        var module = await _context.Modules.Where(m => m.JoinCode == joinCode).FirstOrDefaultAsync();
        if (module == null)
            throw new Exception("Module not found.");

        module.UserModules.Add(new UserModule
        {
            UserId = userId,
            Role = 2
        });
        await _context.SaveChangesAsync();
        return module;
    }

    public async Task<Module> RenameModule(int moduleId, string newName)
    {
        return new Module();
    }

    public async Task<Module> CreateModuleAsync(string ModuleName)
    {
        Module newModule = new Module
        {
            Name = ModuleName,
            JoinCode = Guid.NewGuid().ToString().Substring(0, 6)
        };
        newModule.UserModules.Add(new UserModule
        {
            UserId = _userService.GetCurrentUserId(),
            Role = 0
        });
        _context.Modules.Add(newModule);
        await _context.SaveChangesAsync();
        return newModule;
    }

    public async Task DeleteModuleAsync(int moduleId)
    {
        Module targetModule = await GetModuleAsync(moduleId);

        UserModule moduleOwner = targetModule.UserModules.FirstOrDefault(u => u.Role == 0);

        if (moduleOwner == null)
            throw new Exception("Could not get Module Owner, somehow"); // This should never happen in prod lol

        string? idOfOwner = moduleOwner.UserId;

        // Check user is owner of module
        if (_userService.GetCurrentUserId() != targetModule.UserModules.FirstOrDefault(u => u.Role == 0).UserId)
            throw new Exception("User is not owner of module");

        _context.Modules.Remove(targetModule);
        await _context.SaveChangesAsync();
        return;
    }
}
