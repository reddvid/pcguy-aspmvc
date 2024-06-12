using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class ApplicationUser : IdentityUser
{
    [ProtectedPersonalData] [Required] public string? Name { get; set; }

    [ProtectedPersonalData]
    [DisplayName("Street Address")]
    public string? StreetAddress { get; set; }

    public string? City { get; set; }
    public string? State { get; set; }
    [DisplayName("Postal Code")] public string? PostalCode { get; set; }

    [NotMapped] public string? Role { get; init; }

    public int? CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    [ValidateNever]
    public Company? Company { get; init; }
}