using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.Common.Entities;

namespace PCGuy.Mvc.Models;

public class ProductViewModel
{
    public int CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public Product Product { get; init; } = default!;
    public IEnumerable<SelectListItem> CategoryListItems { get; init; } = default!;
    public IEnumerable<SelectListItem> BrandListItems { get; init; } = default!;
    public IEnumerable<SelectListItem> SubcategoryListItems { get; init; } = default!;
}