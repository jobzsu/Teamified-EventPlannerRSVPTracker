using Microsoft.EntityFrameworkCore.Storage;

namespace EventPlannerRSVPTracker.App.Abstractions.Persistence;

public interface IUnitOfWork
{
    void SaveChanges();

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    IDbContextTransaction BeginTransaction();
}
