using FeedTrac.Server.Database;
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

        return await _context.Modules.Where(m => m.UserModules.Any(u => u.User.Id == userId)).ToListAsync();
    }

    public async Task<Module> GetModuleAsync(int id)
    {
        var module = await _context.Modules.Where(m => m.Id == id).Include(m => m.UserModules).FirstOrDefaultAsync();
        if (module == null)
            throw new Exception("Module not found.");

        return module;
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
}
