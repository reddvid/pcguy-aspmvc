using PCGuy.DataAccess.Contracts;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public ICategoryRepository Category { get; }
    public IProductRepository Product { get; }
    public ISubcategoryRepository Subcategory { get; }
    public IBrandRepository Brand { get; }
    public ICompanyRepository Company { get; }
    public IShoppingCartRepository ShoppingCart { get; }
    public IApplicationUserRepository ApplicationUser { get; }
    public IOrderHeaderRepository OrderHeader { get; }
    public IOrderDetailRepository OrderDetail { get; }
    public IProductImageRepository ProductImage { get; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        Product = new ProductRepository(_db);
        Subcategory = new SubcategoryRepository(_db);
        Brand = new BrandRepository(_db);
        Company = new CompanyRepository(_db);
        ShoppingCart = new ShoppingCartRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        OrderHeader = new OrderHeaderRepository(_db);
        OrderDetail = new OrderDetailRepository(_db);
        ProductImage = new ProductImageRepository(_db);
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}