using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Data;
using PCGuy.Mvc.Models;

namespace PCGuy.Mvc.Controllers;

public class ProductController(ApplicationDbContext context) : Controller
{
    // GET
    public async Task<IActionResult> Index(int id)
    {
        Console.WriteLine("ID" + id);
        var filteredProducts = await context.Products
            .Include("Subcategory")
            .Include("Subcategory.Category")
            .Where(p => p.Subcategory.Category.Id == id)
            .ToListAsync();

        ViewData["Title"] = id switch
        {
            1 => "Software",
            2 => "PC Parts",
            3 => "Peripherals",
            4 => "Accessories",
            _ => ViewData["Title"]
        };

        return View(filteredProducts);
    }

    public IActionResult Create()
    {
        var productViewModel = new ProductViewModel
        {
            Categories = GetCategories(),
            Brands = GetBrands(),
            Subcategories = GetSubcategories()
        };

        return View(productViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        if (viewModel.Product is null) return NotFound();
        
        await context.Products.AddAsync(viewModel.Product);
        await context.SaveChangesAsync();

        TempData["success"] = "Product added successfully";

        int idProducts = GetReturnId(viewModel.Product.SubcategoryId);
        
        return RedirectToAction("Index", new { id = idProducts });
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product is null) return NotFound();
        
        context.Products.Remove(product);
        await context.SaveChangesAsync();

        int idProducts = GetReturnId(product.SubcategoryId);
        
        TempData["success"] = "Product deleted successfully";
        
        return RedirectToAction("Index", new { id = idProducts });
    }

    private int GetReturnId(int productSubcategoryId)
    {
        return context.Subcategories
            .Include("Category")
            .FirstOrDefault(p => p.Id == productSubcategoryId)!
            .CategoryId;
    }

    // ref: https://stackoverflow.com/questions/18382311/populating-a-razor-dropdownlist-from-a-listobject-in-mvc

    private IEnumerable<SelectListItem> GetCategories()
    {
        var categories = context.Categories
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        return categories;
    }

    private IEnumerable<SelectListItem> GetBrands()
    {
        var brands = context.Brands
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        return brands;
    }

    private IEnumerable<SelectListItem> GetSubcategories()
    {
        var subCategories = context.Subcategories
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        return subCategories;
    }
    
    public async Task<IActionResult> Details(int? id)
    {
        var products = await context.Products.Include("Brand").Include("Subcategory").ToListAsync();
        var product = products.Find(x => x.Id == id);
        return View(product);
    }
}