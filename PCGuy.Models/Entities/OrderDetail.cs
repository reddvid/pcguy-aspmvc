using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class OrderDetail
{
    public int Id { get; set; }
    
    [Required] public required int OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    [ValidateNever]
    public OrderHeader? OrderHeader { get; set; }
    
    [Required] public required int ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product? Product { get; set; }

    public int Count { get; set; }
    public double Price { get; set; }
}