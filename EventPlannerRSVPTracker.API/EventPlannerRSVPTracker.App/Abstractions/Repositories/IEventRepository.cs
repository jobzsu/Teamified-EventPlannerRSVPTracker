using EventPlannerRSVPTracker.Domain.Models;

namespace EventPlannerRSVPTracker.App.Abstractions.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<List<Event>?> GetUserEvents(string username, CancellationToken cancellationToken = default, bool shouldTrack = false);

    Task<Event?> GetEventById(Guid eventId, CancellationToken cancellationToken = default, bool shouldTrack = false);
}
