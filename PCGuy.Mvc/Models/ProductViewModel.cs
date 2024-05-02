using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.Common.Entities;

namespace PCGuy.Mvc.Models;

public class ProductViewModel
{
    public Product Product { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; }
    public IEnumerable<SelectListItem> Brands { get; set; }
    public IEnumerable<SelectListItem> Subcategories { get; set; }
}