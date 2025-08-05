using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.Database.DbContext;
using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerRSVPTracker.Database.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    private readonly AppDbContext _dbContext;

    public EventRepository(AppDbContext dbContext) : base(dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

        _dbContext = dbContext;
    }

    public async Task<Event?> GetEventById(Guid eventId, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
                    .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken) :
            await GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }

    public async Task<List<Event>?> GetPublicEvents(CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
                    .ToListAsync(cancellationToken) :
            await GetAll()
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
    }

    public async Task<List<Event>?> GetUserEvents(string username, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
                    .Include(e => e.Host)
                    .AsSplitQuery()
                    .Where(e => e.Host.Username == username)
                    .ToListAsync(cancellationToken) :
            await GetAll()
                    .Include(e => e.Host)
                    .AsSplitQuery()
                    .Where(e => e.Host.Username == username)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
    }
}
