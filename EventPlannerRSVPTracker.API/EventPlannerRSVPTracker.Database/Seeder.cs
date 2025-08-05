using EventPlannerRSVPTracker.Database.DbContext;
using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EventPlannerRSVPTracker.Database;

public static class Seeder
{
    public static void SeedData(AppDbContext dbContext, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        dbContext.Database.EnsureCreated();

        bool hasInserted = false;

        using var txn = dbContext.Database.BeginTransaction();

        try
        {
            var users = dbContext.Users.ToList();

            if (users is not null && users.Count() > 0)
                logger.LogInformation($"[SEEDER] Users table already seeded");
            else
            {
                var user1 = User.Create("user1");

                dbContext.Users.Add(user1);

                hasInserted = true;

                dbContext.SaveChanges();

                txn.Commit();

                logger.LogInformation($"[SEEDER] Users table seeded");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[SEEDER] An error occurred while seeding the database.");

            if (hasInserted)
                txn.Rollback();
        }
    }
}
