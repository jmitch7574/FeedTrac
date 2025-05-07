#nullable disable //suppress null warnings

using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
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

    public virtual bool UserIsInModule(ApplicationUser user, int moduleId)
    {
        //used to mock an unauthorised user for ImageControllerTests
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns all modules that a user has access to
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<Module>> GetUserModulesAsync()
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

        return modules;
    }

    /// <summary>
    /// Gets all modules
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<Module>> GetAllModulesAsync()
    {
        await _userManager.RequireUser("Admin");
        
        // Get all applicable modules
        List<Module> modules = await _context.Modules.ToListAsync();

        return modules;
    }

/*
    /// <summary>
    /// Gets module by specific ID
    /// </summary>
    /// <param name="id">ID of the module to get</param>
    /// <returns></returns>
    /// <exception cref="Exception">Module cannot be found</exception>
    public async Task<Module> GetModuleAsync(int id)
    {
        var module = await _context.Modules.Where(m => m.Id == id)
            .Include(m => m.StudentModule)
            .FirstOrDefaultAsync();

        if (module == null)
            throw new Exception("Module not found.");

        return module;
    }
*/
}
