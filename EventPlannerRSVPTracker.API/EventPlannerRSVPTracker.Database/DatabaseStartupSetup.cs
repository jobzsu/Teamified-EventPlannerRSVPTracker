using EventPlannerRSVPTracker.App.Abstractions.Persistence;
using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.Database.DbContext;
using EventPlannerRSVPTracker.Database.Persistence;
using EventPlannerRSVPTracker.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EventPlannerRSVPTracker.Database;

public static class DatabaseStartupSetup
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, string connString)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connString, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable($"{HistoryRepository.DefaultTableName}_EPRTDb");
            });
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
