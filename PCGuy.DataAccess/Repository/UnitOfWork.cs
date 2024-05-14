using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public ICategoryRepository Category { get; }
    public IProductRepository Product { get; }
    public ISubcategoryRepository Subcategory { get; }
    public IBrandRepository Brand { get; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        Product = new ProductRepository(_db);
        Subcategory = new SubcategoryRepository(_db);
        Brand = new BrandRepository(_db);
    }

    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}