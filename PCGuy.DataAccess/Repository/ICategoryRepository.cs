using PCGuy.Common.Entities;

namespace PCGuy.DataAccess.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
}