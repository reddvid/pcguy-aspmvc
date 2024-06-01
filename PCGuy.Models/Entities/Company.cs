using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class Company
{
    public int Id { get; init; }
    [Required] public string? Name { get; init; }

    [DisplayName("Street Address")] public string? StreetAddress { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    [DisplayName("Postal Code")] public string? PostalCode { get; init; }
    [DisplayName("Phone Number")] public string? PhoneNumber { get; init; }
   
}