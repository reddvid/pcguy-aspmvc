using PCGuy.Common.Entities;

namespace PCGuy.DataAccess.Repository;

public interface ISubcategoryRepository : IRepository<Subcategory>
{
    void Update(Subcategory subcategory);
}