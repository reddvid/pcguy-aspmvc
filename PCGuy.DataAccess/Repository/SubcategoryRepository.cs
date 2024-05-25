using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class SubcategoryRepository(ApplicationDbContext db) : Repository<Subcategory>(db), ISubcategoryRepository
{
    private readonly ApplicationDbContext _db = db;
    
    public void Update(Subcategory subcategory) => _db.Subcategories.Update(subcategory);
}