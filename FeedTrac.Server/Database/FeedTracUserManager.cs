using FeedTrac.Server.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

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
        ILogger<UserManager<ApplicationUser>> logger)
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
}
