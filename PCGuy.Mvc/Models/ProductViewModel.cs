using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.Common.Entities;

namespace PCGuy.Mvc.Models;

public class ProductViewModel
{
    public int? Id { get; init; }
    public required Product Product { get; set; }
    public IEnumerable<SelectListItem> CategoryListItems { get; init; } = default!;
    public IEnumerable<SelectListItem> BrandListItems { get; init; } = default!;
    public IEnumerable<SelectListItem> SubcategoryListItems { get; init; } = default!;
}