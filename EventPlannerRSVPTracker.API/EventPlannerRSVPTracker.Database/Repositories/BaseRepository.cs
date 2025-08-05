using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.Database.DbContext;

namespace EventPlannerRSVPTracker.Database.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

        _context = dbContext;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => Delete(entity), cancellationToken);
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>();
    }

    public async Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => GetAll(), cancellationToken);
    }

    public T Insert(T entity)
    {
        return InsertAsync(entity).Result;
    }

    public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<T>> BulkInsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>()
            .AddRangeAsync(entities, cancellationToken);

        return entities;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);

        return entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => Update(entity), cancellationToken);
    }
}
