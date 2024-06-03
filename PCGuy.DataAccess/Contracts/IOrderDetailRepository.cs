using PCGuy.DataAccess.Repository;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail order);
}