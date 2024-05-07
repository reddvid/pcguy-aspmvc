using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> _dbSet;
    
    
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }
    
    public async Task<IEnumerable<T>> GetAll()
    {
        IQueryable<T> query = _dbSet;
        return await query.ToListAsync();
    }

    public async Task<T> Get(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _dbSet;
        query = query.Where(filter);
        return await query.FirstOrDefaultAsync();
    }

    public async Task Add(T entity)
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