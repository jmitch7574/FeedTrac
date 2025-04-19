using FeedTrac.Server.Database;
using FeedTrac.Server.Models.Responses.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FeedTrac.Server.Services;

public class ModuleService
{
    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ModuleService(ApplicationDbContext context, UserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Module>> GetUserModulesAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new Exception("User is not logged in.");

        return await _context.Modules.Where(m => m.StudentModule.Any(u => u.User.Id == userId) || m.TeacherModule.Any(u => u.User.Id == userId)).ToListAsync();
    }

    public async Task<Module> GetModuleAsync(int id)
    {
        var module = await _context.Modules.Where(m => m.Id == id)
            .Include(m => m.StudentModule)
            .FirstOrDefaultAsync();

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

        if (module.StudentModule.Find(sm => sm.UserId == userId && sm.Module == module) != null)
            throw new Exception("User is already a part of the module");

        module.StudentModule.Add(new StudentModule
        {
            UserId = userId,
            ModuleId = module.Id
        });
        await _context.SaveChangesAsync();
        return module;
    }

    public async Task<Module> RenameModule(int moduleId, string newName)
    {
        Module module = await _context.Modules.FindAsync(moduleId);
        if (module == null)
            throw new Exception("Module not found.");

        module.Name = newName;
        _context.Modules.Update(module);
        await _context.SaveChangesAsync();

        return module;
    }

    public async Task<Module> CreateModuleAsync(string ModuleName)
    {
        Module newModule = new Module
        {
            Name = ModuleName,
            JoinCode = Guid.NewGuid().ToString().Substring(0, 6)
        };

        _context.Modules.Add(newModule);
        await _context.SaveChangesAsync();
        return newModule;
    }

    public async Task DeleteModuleAsync(int moduleId)
    {
        Module targetModule = await GetModuleAsync(moduleId);

        _context.Modules.Remove(targetModule);
        await _context.SaveChangesAsync();
        return;
    }
}
