using FeedTrac.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server.Extensions
{
    /// <summary>
    /// Migration Service for FeedTrac Database Migrations
    /// </summary>
    public static class MigrationExtensions
    {
        /// <summary>
        /// Custom logic for applying migrations
        /// </summary>
        /// <param name="app"></param>
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }
}
