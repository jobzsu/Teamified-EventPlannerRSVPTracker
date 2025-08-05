

using EventPlannerRSVPTracker.Database.ModelConfigurations;
using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerRSVPTracker.Database.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Event> Events => Set<Event>();
}
