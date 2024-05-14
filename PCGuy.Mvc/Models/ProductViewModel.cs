using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.Common.Entities;

namespace PCGuy.Mvc.Models;

public class ProductViewModel
{
    public int CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public Product Product { get; init; } = default!;
    public IEnumerable<SelectListItem> Categories { get; init; } = default!;
    public IEnumerable<SelectListItem> Brands { get; init; } = default!;
    public IEnumerable<SelectListItem> Subcategories { get; init; } = default!;
}