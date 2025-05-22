using Microsoft.EntityFrameworkCore;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.API.Extensions
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is created and migrations are applied
            await context.Database.MigrateAsync();

            // Seed StatusTypes if they don't exist
            if (!await context.StatusTypes.AnyAsync())
            {
                var statusTypes = new List<StatusType>
                {
                    new StatusType { Name = "Available" },
                    new StatusType { Name = "In-Use" },
                    new StatusType { Name = "Deprecated" },
                    new StatusType { Name = "Planned" }
                };

                await context.StatusTypes.AddRangeAsync(statusTypes);
                await context.SaveChangesAsync();
            }
        }
    }
} 