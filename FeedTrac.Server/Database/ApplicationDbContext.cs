using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FeedTrac.Server.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Module> Modules { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<FeedbackTicket> FeedbackTicket { get; set; }
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
