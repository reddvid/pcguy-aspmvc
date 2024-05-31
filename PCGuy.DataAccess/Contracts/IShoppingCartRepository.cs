using PCGuy.DataAccess.Repository;
using PCGuy.Entities.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    void Update(ShoppingCart cart);
}