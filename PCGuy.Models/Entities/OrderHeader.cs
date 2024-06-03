using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class OrderHeader
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser? ApplicationUser { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime ShippingDate { get; set; }
    public double OrderTotal { get; set; }

    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }

    public DateTime PaymentDate { get; set; }
    public DateOnly PaymentDueDate { get; set; }

    public string? PaymentIntentId { get; set; }
    
    [Required] public required string PhoneNumber { get; set; }
    [Required] public required string StreetAddress { get; set; }
    [Required] public required string City { get; set; }
    [Required] public required string State { get; set; }
    [Required] public required string PostalCode { get; set; }
    [Required] public required string Name { get; set; }
}