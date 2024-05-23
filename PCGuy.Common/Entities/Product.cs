using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Common.Entities;

public class Product
{
    [Key] public int Id { get; init; }
    public DateTime UploadDate { get; private set; } = DateTime.Now;
    [DisplayName("Featured Image")]
    [ValidateNever] public string? FeaturedImage { get; set; }
    public string[]? ImagePaths { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Specifications { get; init; }
    [Required] public float Price { get; init; }
    public double Discount { get; init; }
    public bool IsOnSale => Discount > 0;
   
    [DisplayName("Brand Name")]
    public int BrandId { get; init; }

    [ForeignKey("BrandId")]
    [ValidateNever]
    public Brand? Brand { get; init; }

    [DisplayName("Subcategory")]
    public int SubcategoryId { get; init; }

    [ForeignKey("SubcategoryId")]
    [ValidateNever]
    public Subcategory? Subcategory { get; init; }

    [DisplayName("Model Name")]
    [ValidateNever]
    public string? ModelName { get; init; }

    [DisplayName("Model Number")]
    [ValidateNever]
    public string? ModelNumber { get; init; }

    [ValidateNever] public string[]? Tags { get; init; }
}