using PCGuy.Common.Entities;

namespace PCGuy.DataAccess.Repository;

public interface IProductRepository : IRepository<Product>
{
    void Update();
    void Save();
}