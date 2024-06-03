using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class OrderDetailRepository(ApplicationDbContext db) : Repository<OrderDetail>(db), IOrderDetailRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(OrderDetail order) => _db.OrderDetails.Update(order);
}