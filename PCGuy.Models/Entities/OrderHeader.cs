using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class OrderHeader
{
    public int Id { get; init; }
    public string? ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser? ApplicationUser { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime ShippingDate { get; init; }
    public double OrderTotal { get; set; }

    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TrackingNumber { get; init; }
    public string? Carrier { get; init; }

    public DateTime PaymentDate { get; set; }
    public DateOnly PaymentDueDate { get; init; }

    public string? SessionId { get; set; }
    public string? PaymentIntentId { get; set; }
    
    [Required] public string? PhoneNumber { get; set; }
    [Required] public string? StreetAddress { get; set; }
    [Required] public string? City { get; set; }
    [Required] public string? State { get; set; }
    [Required] public string? PostalCode { get; set; }
    [Required] public string? Name { get; set; }

}