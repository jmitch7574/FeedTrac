using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FeedTrac.Server.Database
{
    /// <summary>
    /// The Database context of FeedTrac
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// The modules table
        /// </summary>
        public DbSet<Module> Modules { get; set; }

        /// <summary>
        /// The user-to-modules table
        /// </summary>
        public DbSet<UserModule> UserModules { get; set; }

        /// <summary>
        /// The feedback tickets table
        /// </summary>
        public DbSet<FeedbackTicket> FeedbackTicket { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Configures the database schema
        /// </summary>
        /// <param name="builder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("feedtrac");
            builder.Entity<ApplicationUser>().Ignore(c => c.EmailConfirmed)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.AccessFailedCount)
                .Ignore(c => c.TwoFactorEnabled);

            builder.Entity<ApplicationUser>()
                .Property(e => e.FirstName)
                .HasMaxLength(250);

            builder.Entity<ApplicationUser>()
                .Property(e => e.LastName)
                .HasMaxLength(250);

            builder.Entity<UserModule>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserModules) // Assuming a collection exists in ApplicationUser
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional, specify cascade or restrict

            builder.Entity<UserModule>()
                .HasOne(um => um.Module)
                .WithMany(m => m.UserModules) // Assuming a collection exists in Module
                .HasForeignKey(um => um.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FeedbackTicket>()
                .HasOne(um => um.Owner)
                .WithMany(m => m.Tickets) // Assuming a collection exists in Module
                .HasForeignKey(um => um.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<FeedbackTicket>()
                .HasOne(um => um.Module)
                .WithMany(m => m.Tickets) // Assuming a collection exists in Module
                .HasForeignKey(um => um.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
