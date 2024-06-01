using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PCGuy.Models.Entities;

public class ApplicationUser : IdentityUser
{
    [Required] public string? Name { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    
    [NotMapped] public string? Role { get; init; }
    
    public int? CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    [ValidateNever]
    public Company? Company { get; init; }
}