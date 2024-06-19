using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class ProductImageRepository(ApplicationDbContext db) : Repository<ProductImage>(db), IProductImageRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(ProductImage image) => _db.ProductImages.Update(image);
}