using PCGuy.DataAccess.Repository;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader orderHeader);
    Task UpdateStatusAsync(int id, string orderStatus, string? paymentStatus = null);
    Task UpdateStripePaymentIdAsync(int id, string sessionId, string paymentIntentId);
}