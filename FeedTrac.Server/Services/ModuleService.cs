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
    private readonly FeedTracUserManager _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ModuleService(ApplicationDbContext context, FeedTracUserManager userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

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

        return modules;
    }

    public async Task<List<Module>> GetAllModulesAsync()
    {
        await _userManager.RequireUser("Admin");
        
        // Get all applicable modules
        List<Module> modules = await _context.Modules.ToListAsync();

        return modules;
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
}
