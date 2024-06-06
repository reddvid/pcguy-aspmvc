using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class OrderDetail
{
    public int Id { get; init; }
    
    [Required] public required int OrderHeaderId { get; init; }

    [ForeignKey("OrderHeaderId")]
    [ValidateNever]
    public OrderHeader OrderHeader { get; init; } = default!;
    
    [Required] public required int ProductId { get; init; }

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; init; } = default!;

    public int Count { get; init; }
    public double Price { get; init; }
}