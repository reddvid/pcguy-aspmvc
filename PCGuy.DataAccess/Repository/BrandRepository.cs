using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class BrandRepository(ApplicationDbContext db) : Repository<Brand>(db), IBrandRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Brand brand) => _db.Brands.Update(brand);
}