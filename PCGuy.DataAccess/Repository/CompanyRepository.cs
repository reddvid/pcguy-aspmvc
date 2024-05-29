using PCGuy.DataAccess.Contracts;
using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class CompanyRepository(ApplicationDbContext db) : Repository<Company>(db), ICompanyRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Company company) => _db.Companies.Update(company);
}