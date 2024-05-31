using PCGuy.DataAccess.Contracts;
using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.DataAccess.Repository;

public class ShoppingCartRepository(ApplicationDbContext db) : Repository<ShoppingCart>(db), IShoppingCartRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(ShoppingCart cart) => _db.ShoppingCarts.Update(cart);
}