using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace FeedTrac.Server.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Module> Modules { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("feedtrac");
            builder.Entity<ApplicationUser>().Ignore(c => c.EmailConfirmed)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.AccessFailedCount);

            builder.Entity<ApplicationUser>()
                .HasMany<Module>(s => s.Modules)
                .WithMany(c => c.Users);
        }

    }
}
