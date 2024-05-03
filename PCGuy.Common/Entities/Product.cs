using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCGuy.Common.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.Now;
    public string[]? ImagePaths { get; set; }
    public required string? Name { get; set; } 
    public string? Description { get; set; } 
    public string? Specifications { get; set; }
    public required float Price { get; set; }
    public double Discount { get; set; }
    public bool IsOnSale => Discount > 0;
    [ForeignKey("BrandId")]
    [DisplayName("Brand")]
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    [ForeignKey("SubcategoryId")]
    [DisplayName("Subcategory")]
    public int SubcategoryId { get; set; }
    public Subcategory Subcategory { get; set; }

    [DisplayName("Model Name")] public string? ModelName { get; set; } 
    [DisplayName("Model Number")] public string? ModelNumber { get; set; } 
    public string[]? Tags { get; set; }
}