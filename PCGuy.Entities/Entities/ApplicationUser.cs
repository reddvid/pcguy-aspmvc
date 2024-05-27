using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PCGuy.Entities.Entities;

public class ApplicationUser : IdentityUser
{
    [Required] public string? Name { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    [NotMapped] public string? Role { get; set; }
}