using System.Globalization;
using PCGuy.Models.Entities;

namespace PCGuy.Models.ViewModels;

public class ShoppingCartViewModel
{
    public IEnumerable<ShoppingCart>? ShoppingCartList { get; set; }
    public double OrderTotal { get; set; }
    public string DisplayOrderTotal => OrderTotal.ToString("C", new CultureInfo("fil-PH"));
}