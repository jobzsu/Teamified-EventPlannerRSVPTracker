using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.Database.DbContext;
using EventPlannerRSVPTracker.Domain.Models;

namespace EventPlannerRSVPTracker.Database.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    private readonly AppDbContext _dbContext;

    public EventRepository(AppDbContext dbContext) : base(dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

        _dbContext = dbContext;
    }
}
