using FeedTrac.Server.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FeedTrac.Server.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly FeedTracUserManager _userManager;

    public UserService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,  FeedTracUserManager userManager)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    /// <summary>
    /// Function that can be used within endpoints to require a user be logged in with optional roles.
    /// An Exception will be thrown and caught by the middleware if a user does not meet the criteria
    /// </summary>
    /// <param name="roles">The roles the user requires</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="NotLoggedInException"></exception>
    /// <exception cref="InsufficientRolesException"></exception>
    public async Task<ApplicationUser> RequireUser(params string[] roles)
    {
        if (_httpContextAccessor.HttpContext?.User.Identity is null)
            throw new Exception("HttpContext is null");
        
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            throw new NotLoggedInException();

        ApplicationUser? user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        
        if (user == null)
            throw new Exception("User not found");
        
        var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
        foreach (var role in roles)
        {
            if (!(await _userManager.IsInRoleAsync(user, role)))
                throw new InsufficientRolesException();
        }
        
        return user;
    }
    public string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new Exception("User is not logged in.");
        return userId;
    }
    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var userId = GetCurrentUserId();
        return await GetUserByIdAsync(userId);
    }
    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
