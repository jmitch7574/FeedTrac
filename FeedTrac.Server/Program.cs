using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace FeedTrac.Server
{
    /// <summary>
    /// The entry class of the project
    /// </summary>
    public abstract class Program
    {
        public static string TEST_ADMIN_2FA = "VMNAYBBTP4PHHMNF53O2W5UGRJDD442G";
        
        /// <summary>
        /// Create our custom roles
        /// </summary>
        /// <param name="serviceProvider">Service provider made in main</param>
        /// <returns></returns>
        static async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Teacher", "Admin", "Student" };
            foreach (var roleName in roleNames)
            {
                bool roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }


        static async Task CreateDefaultAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = await userManager.FindByEmailAsync("feedtrac-admin@lincoln.ac.uk");
            if (admin != null)
            {
                // Check the admins 2FA secret is correct
                admin.TwoFactorSecret = TEST_ADMIN_2FA;
                await userManager.UpdateAsync(admin);
                return;
            }

            admin = new ApplicationUser
            {
                UserName = "feedtrac-admin@lincoln.ac.uk",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "feedtrac-admin@lincoln.ac.uk"
            };

            var result = await userManager.CreateAsync(admin, "Password123!");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return;
            }


            await userManager.AddToRoleAsync(admin, "Admin");
            admin.TwoFactorSecret = TEST_ADMIN_2FA;


            await userManager.UpdateAsync(admin);
        }

        /// <summary>
        /// Entry point of the program
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            // Load our environment variables
            EnvironmentVariables.Load();
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen(options =>
            {
                // Enable XML comments (next step)
                var xmlFilename = "docs.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("database")));

            // Establish Connection to SQL Server
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ModuleService>();
            builder.Services.AddScoped<ImageService>();
            builder.Services.AddScoped<FeedTracUserManager>();
            builder.Services.AddScoped<EmailService>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false; // Optional
                options.SignIn.RequireConfirmedPhoneNumber = false; // Optional
                options.Tokens.AuthenticatorTokenProvider = null!; // Remove 2FA providers
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None; // Cross-origin cookie allowed
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS required
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Localhost", policy =>
                {
                    policy.WithOrigins("http://localhost:5174")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("Localhost");
            app.UseMiddleware<FeedTracMiddleware>(); // Register your custom middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.ApplyMigrations();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            

            app.UseAuthorization();

            app.MapFallbackToFile("/index.html");

            // Ensure roles exist
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await CreateRolesAsync(services);
                await CreateDefaultAdministrator(services);
            }

            app.Run();
        }
    }
}
