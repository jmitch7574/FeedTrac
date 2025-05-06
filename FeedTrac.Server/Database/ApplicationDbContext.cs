using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<Module> Modules { get; set; }

        /// <summary>
        /// Many to Many collection that links Modules and Students
        /// </summary>
        public virtual DbSet<StudentModule> StudentModules { get; set; }

        /// <summary>
        /// Many to Many collection that links Modules and Teachers
        /// </summary>
        public virtual DbSet<TeacherModule> TeacherModules { get; set; }

        /// <summary>
        /// Tickets Table
        /// </summary>
        public virtual DbSet<FeedbackTicket> Tickets { get; set; }

        /// <summary>
        /// Messages Table
        /// </summary>
        public virtual DbSet<FeedbackMessage> Messages { get; set; }

        /// <summary>
        /// Images table
        /// </summary>
        public virtual DbSet<MessageImage> Images { get; set; }

        /// <summary>
        /// The feedback tickets table
        /// </summary>
        public virtual DbSet<FeedbackTicket> FeedbackTicket { get; set; }
        
        /// <summary>
        /// DBContext Constructor
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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

            builder.Entity<StudentModule>()
                .HasOne(um => um.User)
                .WithMany(u => u.EnrolledModules) // Assuming a collection exists in ApplicationUser
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional, specify cascade or restrict

            builder.Entity<StudentModule>()
                .HasOne(um => um.Module)
                .WithMany(m => m.StudentModule) // Assuming a collection exists in Module
                .HasForeignKey(um => um.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<TeacherModule>()
                .HasOne(um => um.User)
                .WithMany(u => u.TeachingModules) // Assuming a collection exists in ApplicationUser
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional, specify cascade or restrict

            builder.Entity<TeacherModule>()
                .HasOne(um => um.Module)
                .WithMany(m => m.TeacherModule) // Assuming a collection exists in Module
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

            builder.Entity<FeedbackMessage>()
                .HasOne(fm => fm.Ticket)
                .WithMany(ft => ft.Messages)
                .HasForeignKey(fm => fm.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MessageImage>()
                .HasOne(im => im.Message)
                .WithMany(fm => fm.Images)
                .HasForeignKey(im => im.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
