using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.Models.Entities;

namespace PCGuy.Models.ViewModels;

public class ManageUserViewModel
{
    public ApplicationUser? ApplicationUser { get; init; }
    public IEnumerable<SelectListItem>? RolesList { get; init; }
    public IEnumerable<SelectListItem>? CompanyList { get; init; }
}