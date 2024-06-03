using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class ShoppingCart
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    [ForeignKey("ProductId")] public required Product Product { get; init; }
    [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
    public int Count { get; set; }
    public string? ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")] [ValidateNever] public ApplicationUser? ApplicationUser { get; init; }
 
}