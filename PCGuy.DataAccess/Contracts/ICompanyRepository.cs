using PCGuy.DataAccess.Repository;
using PCGuy.Entities.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company company);
}