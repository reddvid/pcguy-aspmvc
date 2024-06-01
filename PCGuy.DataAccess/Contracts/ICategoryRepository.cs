using PCGuy.DataAccess.Repository;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
}