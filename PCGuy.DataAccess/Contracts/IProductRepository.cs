using PCGuy.DataAccess.Repository;
using PCGuy.Entities.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}