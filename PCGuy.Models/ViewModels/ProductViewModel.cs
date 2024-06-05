using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.Models.Entities;

namespace PCGuy.Models.ViewModels;

public class ProductViewModel
{
    public int Id { get; init; }
    public required Product Product { get; set; }
    [ValidateNever] public IEnumerable<SelectListItem> BrandListItems { get; set; } = default!;
    [ValidateNever] public IEnumerable<SelectListItem> SubcategoryListItems { get; set; } = default!;
}