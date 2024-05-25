using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Product product) => _db.Products.Update(product);
}