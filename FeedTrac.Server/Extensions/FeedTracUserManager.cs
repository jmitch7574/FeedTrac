using FeedTrac.Server.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public class FeedTracUserManager : UserManager<ApplicationUser>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="optionsAccessor"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="userValidators"></param>
    /// <param name="passwordValidators"></param>
    /// <param name="keyNormalizer"></param>
    /// <param name="errors"></param>
    /// <param name="services"></param>
    /// <param name="logger"></param>
    public FeedTracUserManager(
        IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    /// <summary>
    /// An override function for creating a user, allows us to add our email extention validation rules
    /// </summary>
    /// <param name="user">The <see cref="ApplicationUser"/></param>
    /// <param name="password">The user's password</param>
    /// <returns></returns>
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
}
