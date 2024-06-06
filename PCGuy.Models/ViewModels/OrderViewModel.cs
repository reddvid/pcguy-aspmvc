using PCGuy.Models.Entities;

namespace PCGuy.Models.ViewModels;

public class OrderViewModel
{
    public OrderHeader OrderHeader { get; set; } = default!;
    public IEnumerable<OrderDetail> OrderDetails { get; set; } = default!;
}