using PCGuy.DataAccess.Repository;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}