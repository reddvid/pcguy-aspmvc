using PCGuy.Entities.Entities;

namespace PCGuy.DataAccess.Repository;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}