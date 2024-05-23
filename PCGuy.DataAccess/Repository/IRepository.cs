using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace PCGuy.DataAccess.Repository;

public interface IRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);
    public Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
    public Task AddAsync(T entity);
    public void Remove(T entity);
    public void RemoveRange(IEnumerable<T> entities);
}