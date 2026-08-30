using Microsoft.EntityFrameworkCore;
using University.Identity;
using University.Persistance.Context;

namespace University.API.Utils
{
    public static class DatabaseExtensions
    {
        public static async Task ApplyMigrationAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvier = scope.ServiceProvider;
            var logger = serviceProvier.GetRequiredService<ILogger<Program>>();

            try
            {
                var universityDb = serviceProvier.GetRequiredService<UniversityDbContext>();
                await universityDb.Database.GetAppliedMigrationsAsync();

                var univerItendityDb = serviceProvier.GetRequiredService<UniversityIdentityDbContext>();
                await univerItendityDb.Database.GetAppliedMigrationsAsync();

                logger.LogInformation("All migrations are applied successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred while applying database migrations.");

            }
        }
    }
}
