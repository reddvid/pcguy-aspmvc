using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class OrderHeaderRepository(ApplicationDbContext db) : Repository<OrderHeader>(db), IOrderHeaderRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(OrderHeader orderHeader) => _db.OrderHeaders.Update(orderHeader);

    public async Task UpdateStatusAsync(int id, string orderStatus, string? paymentStatus = null)
    {
        var order = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return;
        order.OrderStatus = orderStatus;

        if (string.IsNullOrEmpty(paymentStatus)) return;
        order.PaymentStatus = paymentStatus;
    }

    public async Task UpdateStripePaymentIdAsync(int id, string sessionId, string paymentIntentId)
    {
        var order = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return;
        
        if (string.IsNullOrEmpty(sessionId)) return;
        order.SessionId = sessionId;

        if (string.IsNullOrEmpty(paymentIntentId)) return;
        order.PaymentIntentId = paymentIntentId;
        order.PaymentDate = DateTime.Now;
    }
}