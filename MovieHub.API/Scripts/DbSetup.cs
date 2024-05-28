using Microsoft.EntityFrameworkCore;
using MovieHub.DbContexts;

namespace MovieHub.Scripts;

public class DbSetup
{
    public static void SeedDatabase(IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<MovieHubContext>();
        var logger = services.GetRequiredService<ILogger<DbSetup>>();
        
        logger.LogInformation("Verifying database.");
        
        context.Database.EnsureCreated();
        
        logger.LogInformation("Database created.");

        if (context.Movies.Any() || context.Cinemas.Any() || context.MovieCinemas.Any())
        {
            logger.LogInformation("Database already seeded.");
            return;
        };

        var sqlFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Scripts",
            "moviehub-db-data-seed.sql"
        );

        if (!File.Exists(sqlFile)) throw new FileNotFoundException(sqlFile);
    
        logger.LogInformation("Seeding database.");

        var sql = File.ReadAllText(sqlFile);
        context.Database.ExecuteSqlRaw(sql);
        
        logger.LogInformation("Completed seeding database.");
    }
}