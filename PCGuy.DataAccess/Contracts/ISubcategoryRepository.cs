using PCGuy.Entities.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface ISubcategoryRepository : IRepository<Subcategory>
{
    void Update(Subcategory subcategory);
}