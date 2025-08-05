using EventPlannerRSVPTracker.Domain.Models;

namespace EventPlannerRSVPTracker.App.Abstractions.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsername(string username, CancellationToken cancellationToken = default, bool shouldTrack = false);
}
