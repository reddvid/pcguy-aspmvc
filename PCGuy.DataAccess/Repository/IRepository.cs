using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace PCGuy.DataAccess.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> Get(Expression<Func<T, bool>> filter);
    Task Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}