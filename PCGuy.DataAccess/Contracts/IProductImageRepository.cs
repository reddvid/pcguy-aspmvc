using PCGuy.DataAccess.Repository;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IProductImageRepository : IRepository<ProductImage>
{
    void Update(ProductImage image);
}