using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.Database.DbContext;
using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerRSVPTracker.Database.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

        _dbContext = dbContext;
    }

    public async Task<User?> GetByUsername(string username, CancellationToken cancellationToken = default, bool shouldTrack = false)
    {
        return shouldTrack ?
            await GetAll()
                    .FirstOrDefaultAsync(u => u.Username == username, cancellationToken) :
            await GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }
}
