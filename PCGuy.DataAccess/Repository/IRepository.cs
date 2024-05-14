using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace PCGuy.DataAccess.Repository;

public interface IRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAll();
    public IQueryable<T> GetAllAsQuery();
    public Task<T?> Get(Expression<Func<T, bool>> filter);
    public Task Add(T entity);
    public void Remove(T entity);
    public void RemoveRange(IEnumerable<T> entities);
}