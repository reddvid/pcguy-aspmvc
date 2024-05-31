using PCGuy.DataAccess.Repository;

namespace PCGuy.DataAccess.Contracts;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    ISubcategoryRepository Subcategory { get; }    
    IBrandRepository Brand { get; }
    ICompanyRepository Company { get; }
    IShoppingCartRepository ShoppingCart { get; }
    IApplicationUserRepository ApplicationUser { get; }

    Task SaveAsync();
}