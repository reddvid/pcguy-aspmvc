using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface ISubcategoryRepository : IRepository<Subcategory>
{
    void Update(Subcategory subcategory);
}