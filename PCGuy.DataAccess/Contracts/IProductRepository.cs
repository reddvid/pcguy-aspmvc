using PCGuy.DataAccess.Repository;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IProductRepository : IRepository<Product>
{
    Task UpdateAsync(Product product);
}