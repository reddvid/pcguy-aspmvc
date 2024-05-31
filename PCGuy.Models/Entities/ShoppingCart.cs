using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Entities.Entities;

public class ShoppingCart
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product Product { get; set; }
    [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
    public int Count { get; set; }
    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")] [ValidateNever] public ApplicationUser ApplicationUser { get; set; }
}