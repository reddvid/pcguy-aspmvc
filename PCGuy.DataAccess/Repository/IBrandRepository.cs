using System.Linq.Expressions;
using PCGuy.Common.Entities;

namespace PCGuy.DataAccess.Repository;

public interface IBrandRepository : IRepository<Brand>
{
    void Update(Brand brand);
}