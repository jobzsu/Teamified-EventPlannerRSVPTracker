using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.Database.DbContext;
using EventPlannerRSVPTracker.Domain.Models;

namespace EventPlannerRSVPTracker.Database.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

        _dbContext = dbContext;
    }
}
