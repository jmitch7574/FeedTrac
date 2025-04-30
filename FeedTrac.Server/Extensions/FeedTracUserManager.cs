using FeedTrac.Server.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;


namespace FeedTrac.Server.Extensions;

/// <summary>
/// Manager Service for FeedTrac Users
/// </summary>
public class FeedTracUserManager : UserManager<ApplicationUser>
{
    public FeedTracUserManager(
        IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger,
        IHttpContextAccessor httpContextAccessor)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        if (user.Email == null)
        {
            return IdentityResult.Failed(new IdentityError { Code = "Invalid Domain", Description = "Email is required." });
        }
        
        if (!(user.Email.EndsWith("@lincoln.ac.uk") || user.Email.EndsWith("@students.lincoln.ac.uk")))
        {
            return IdentityResult.Failed(new IdentityError { Code = "Invalid Domain", Description = "Only Lincoln domain emails are allowed." });
        }

        return await base.CreateAsync(user, password);
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
            throw new NotLoggedInException();
        
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            throw new NotLoggedInException();

        ApplicationUser? user = await GetUserAsync(_httpContextAccessor.HttpContext.User);
        
        if (user == null)
            throw new ResourceNotFoundException();
        
        if (roles.Length > 0)
            if (! await roles.ToAsyncEnumerable().AnyAwaitAsync(async r => await IsInRoleAsync(user, r)))
                    throw new InsufficientRolesException();
        
        return user;
    }
}
