using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class ApplicationUserRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IApplicationUserRepository
{
    private readonly ApplicationDbContext _db = db;
}