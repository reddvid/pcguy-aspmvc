using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PCGuy.DataAccess.Contracts;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class Repository<T>(ApplicationDbContext db) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = db.Set<T>();
    private readonly char[] _separator = [','];

    public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (string.IsNullOrEmpty(includeProperties)) return await query.ToListAsync();
        query = includeProperties.Split(_separator, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty.Trim()));
        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        query = query.Where(filter);

        if (string.IsNullOrEmpty(includeProperties)) return await query.FirstOrDefaultAsync();

        query = includeProperties.Split(_separator, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty.Trim()));

        return await query.FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}