using PCGuy.Models.Entities;

namespace PCGuy.Models.ViewModels;

public class ShoppingCartViewModel
{
    public IEnumerable<ShoppingCart>? ShoppingCartList { get; set; }
    public double OrderTotal { get; set; }
}