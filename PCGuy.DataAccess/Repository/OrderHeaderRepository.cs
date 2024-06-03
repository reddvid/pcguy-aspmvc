using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class OrderHeaderRepository(ApplicationDbContext db) : Repository<OrderHeader>(db), IOrderHeaderRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(OrderHeader orderHeader) => _db.OrderHeaders.Update(orderHeader);
}