using PCGuy.DataAccess.Repository;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Contracts;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    void Update(ShoppingCart cart);
}