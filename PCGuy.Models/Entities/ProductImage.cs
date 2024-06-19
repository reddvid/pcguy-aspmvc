using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCGuy.Models.Entities;

public class ProductImage
{
    public int Id { get; init; }
    
    [Required]
    public required string ImageUrl { get; init; }

    public int ProductId { get; init; }
    
    [ForeignKey("ProductId")] public Product? Product { get; init; }
}