using users_service.Src.Data;
using Microsoft.EntityFrameworkCore;


namespace users_service.Src.Extensions
{
    public class AppSeedService
    {
        /// <summary>
        /// Calls the seed method to populate the database with example data.
        /// </summary>
        /// <param name="app">Singleton WebApplication</param>
        public static void SeedDatabase(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                // Migrate the database, create if it doesn't exist
                context.Database.Migrate();
                Seed.SeedData(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, " A problem ocurred during seeding ");
            }
        }
    }
}