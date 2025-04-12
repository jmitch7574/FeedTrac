using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FeedTrac.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("database")));

            // Establish Connection to SQL Server
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ModuleService>();
            builder.Services.AddScoped<FeedbackService>();
            builder.Services.AddScoped<UserManager<ApplicationUser>, FeedTracUserManager>();

            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // Replace with your frontend's URL
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.ApplyMigrations();
            }

            app.UseHttpsRedirection();

            // Use CORS before any other middleware that might block the requests
            app.UseCors("AllowFrontend");

            app.MapControllers();

            app.UseAuthorization();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
