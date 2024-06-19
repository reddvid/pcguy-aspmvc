using Microsoft.EntityFrameworkCore;
using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task UpdateAsync(Product product)
    {
        var productFromDb = await _db.Products.FirstOrDefaultAsync(o => o.Id == product.Id);
        if (productFromDb is not null)
        {
            productFromDb.ProductImages = product.ProductImages;
        }
        
        _db.Products.Update(product);
    }
}