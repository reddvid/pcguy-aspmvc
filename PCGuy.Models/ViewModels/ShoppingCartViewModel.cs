using System.Globalization;
using PCGuy.Models.Entities;

namespace PCGuy.Models.ViewModels;

public class ShoppingCartViewModel
{
    public required IEnumerable<ShoppingCart> ShoppingCartList { get; init; }
    public required OrderHeader OrderHeader { get; init; }
    public string? DisplayOrderTotal => OrderHeader?.OrderTotal.ToString("C", new CultureInfo("fil-PH"));
}