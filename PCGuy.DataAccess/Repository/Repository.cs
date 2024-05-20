using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class Repository<T>(ApplicationDbContext db) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = db.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        IQueryable<T> query = _dbSet;
        return await query.ToListAsync();
    }

    public IQueryable<T> GetAllAsQuery()
    {
        return _dbSet;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _dbSet;
        query = query.Where(filter);
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