using EventPlannerRSVPTracker.App.DTOs;

namespace EventPlannerRSVPTracker.App.Abstractions.Services;

public interface IUsersService
{
    Task<ResultModel<Guid>> Login(string username, CancellationToken cancellationToken = default);
}
