using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    ISubcategoryRepository Subcategory { get; }    
    IBrandRepository Brand { get; }

    Task Save();
}