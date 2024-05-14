using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Data;
using PCGuy.DataAccess.Repository;
using PCGuy.Mvc.Models;

namespace PCGuy.Mvc.Controllers;

public class ProductController(IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index(int id)
    {
        Console.WriteLine($"ID {id}");
        
        var filteredProducts = await unitOfWork.Product
            .GetAllAsQuery()
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

        TempData["ProductsViewId"] = id;
        
        return View(filteredProducts);
    }

    public IActionResult Create(int id)
    {
        var productViewModel = new ProductViewModel
        {
            CategoryId = id,
            Categories = GetCategories(),
            Brands = GetBrands(),
            Subcategories = GetSubcategories()
        };
        
        Console.WriteLine(productViewModel.CategoryId);
        
        return View(productViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        await unitOfWork.Product.Add(viewModel.Product);
        await unitOfWork.Save();

        TempData["success"] = "Product added successfully";

        var idProducts = GetReturnId(viewModel.Product.SubcategoryId);
        
        return RedirectToAction("Index", new { id = idProducts });
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var product = await unitOfWork.Product.Get(x => x.Id == id);

        if (product is null) return NotFound();
        
        unitOfWork.Product.Remove(product);
        await unitOfWork.Save();

        int idProducts = GetReturnId(product.SubcategoryId);
        
        TempData["success"] = "Product deleted successfully";
        
        return RedirectToAction("Index", new { id = idProducts });
    }

    private int GetReturnId(int productSubcategoryId)
    {
        var subcategory = unitOfWork.Subcategory
            .GetAllAsQuery()
            .Include("Category")
            .FirstOrDefault(p => p.Id == productSubcategoryId);
        
        return subcategory?.CategoryId ?? 2;
    }

    // ref: https://stackoverflow.com/questions/18382311/populating-a-razor-dropdownlist-from-a-listobject-in-mvc

    private IEnumerable<SelectListItem> GetCategories()
    {
        var categories = unitOfWork.Category
            .GetAllAsQuery()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        return categories;
    }

    private IEnumerable<SelectListItem> GetBrands()
    {
        var brands = unitOfWork.Brand
            .GetAllAsQuery()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        return brands;
    }

    private IEnumerable<SelectListItem> GetSubcategories()
    {
        var subCategories = unitOfWork.Subcategory
            .GetAllAsQuery()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        return subCategories;
    }
    
    public async Task<IActionResult> Details(int? id)
    {
        var products = await unitOfWork.Product
            .GetAllAsQuery()
            .Include("Brand")
            .Include("Subcategory")
            .ToListAsync();
        
        var product = products.Find(x => x.Id == id);
        return View(product);
    }
}