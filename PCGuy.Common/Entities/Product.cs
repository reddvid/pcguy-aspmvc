using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCGuy.Common.Entities;

public class Product
{
    public int Id { get; set; }
    public DateTime UploadDate { get; set; }
    public string[]? ImagePaths { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Specifications { get; set; }
    public float Price { get; set; }
    public double Discount { get; set; }
    public bool IsOnSale => Discount > 0;
    public Brand? Brand { get; set; }
    public Subcategory? SubCategory { get; set; }

    public string? ModelName { get; set; }
    public string? ModelNumber { get; set; }
    public string[]? Tags { get; set; }
}