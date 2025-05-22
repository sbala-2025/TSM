using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // Seed an admin role if it doesn't exist
                    if (!roleManager.RoleExistsAsync("Admin").Result)
                    {
                        var adminRole = new IdentityRole("Admin");
                        roleManager.CreateAsync(adminRole).Wait();
                    }

                    // Seed an admin user if it doesn't exist
                    var adminUser = userManager.FindByNameAsync("admin").Result;
                    if (adminUser == null)
                    {
                        adminUser = new ApplicationUser
                        {
                            UserName = "admin",
                            Email = "admin@tsm.com",
                            FirstName = "Admin",
                            LastName = "User",
                            IsAdmin = true,
                            CreatedAt = DateTime.UtcNow,
                            SecurityStamp = Guid.NewGuid().ToString()
                        };

                        var result = userManager.CreateAsync(adminUser, "Admin123!").Result;
                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing the database.");
                }
            }

            return app;
        }
    }
} 