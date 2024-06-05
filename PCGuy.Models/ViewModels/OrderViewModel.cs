using PCGuy.Models.Entities;

namespace PCGuy.Models.ViewModels;

public class OrderViewModel
{
    public OrderHeader OrderHeader { get; init; } = default!;
    public IEnumerable<OrderDetail> OrderDetails { get; init; } = default!;
}