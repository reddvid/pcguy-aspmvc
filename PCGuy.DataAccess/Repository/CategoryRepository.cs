using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class CategoryRepository(ApplicationDbContext db) : Repository<Category>(db), ICategoryRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Category category) => _db.Categories.Update(category);
}