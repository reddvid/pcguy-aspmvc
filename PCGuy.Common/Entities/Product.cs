using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCGuy.Common.Entities;

public class Product
{
    [Key] public int Id { get; init; }
    public DateTime UploadDate { get; private set; } = DateTime.Now;
    public string? FeaturedImage { get; init; }
    public string[]? ImagePaths { get; init; }
    public required string? Name { get; init; }
    public string? Description { get; init; }
    public string? Specifications { get; init; }
    public required float Price { get; init; }
    public double Discount { get; init; }
    public bool IsOnSale => Discount > 0;

    [ForeignKey("BrandId")]
    [DisplayName("Brand")]
    public int BrandId { get; init; }

    public Brand? Brand { get; init; }

    [ForeignKey("SubcategoryId")]
    [DisplayName("Subcategory")]
    public int SubcategoryId { get; init; }

    public Subcategory? Subcategory { get; init; }

    [DisplayName("Model Name")] public string? ModelName { get; init; }
    [DisplayName("Model Number")] public string? ModelNumber { get; init; }
    public string[]? Tags { get; init; }
}