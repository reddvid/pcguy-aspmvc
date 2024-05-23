using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Repository;
using PCGuy.Mvc.Models;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
{
    [Route("products")]
    public async Task<IActionResult> Index()
    {
        var products = await unitOfWork.Product
            .GetAllAsQuery()
            .Include("Subcategory")
            .Include("Subcategory.Category").ToListAsync();

        ViewData["Title"] = "Products";

        return View(products);
    }

    [Route("products/{id:int}")]
    public async Task<IActionResult> Index(int? id)
    {
        var products = await unitOfWork.Product
            .GetAllAsQuery()
            .Include("Subcategory")
            .Include("Subcategory.Category")
            .ToListAsync();

        var subcategories = await unitOfWork.Subcategory
            .GetAllAsQuery()
            .ToListAsync();

        if (id is null or 0)
        {
            ViewData["Title"] = "All Products";
            return View(products);
        }

        var subcategory = subcategories.Find(o => o.Id == id);
        if (subcategory is null) return View(products);

        var filteredProductsBySubcategory = products.Where(p => p.Subcategory?.Category?.Id == id)
            .ToList();

        ViewData["Title"] = subcategory.Name;
        TempData["ProductSubcategoryId"] = id;

        return View(filteredProductsBySubcategory);
    }

    public async Task<IActionResult> Upsert(int? id)
    {
        var brandList = unitOfWork.Brand
            .GetAllAsQuery()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        var subcategoryList = unitOfWork.Subcategory
            .GetAllAsQuery()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        var productViewModel = new ProductViewModel
        {
            Product = new Product(),
            BrandListItems = brandList,
            SubcategoryListItems = subcategoryList,
        };

        if (id is null or 0)
        {
            // Create
            return View(productViewModel);
        }

        // Edit
        productViewModel.Product = await unitOfWork.Product.GetAsync(o => o.Id == id);

        return View(productViewModel);
    }

    [HttpPost, ActionName("Upsert")]
    public async Task<IActionResult> UpsertPOST(ProductViewModel viewModel, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var wwwRootPath = webHostEnvironment.WebRootPath;
            if (file is not null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(viewModel.Product.FeaturedImage))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, viewModel.Product.FeaturedImage.TrimStart('\\'));
                    
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }

                await using var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create);
                await file.CopyToAsync(fileStream);

                viewModel.Product!.FeaturedImage = @"\images\product\" + fileName;
            }

            if (viewModel.Product.Id == 0)
            {
                await unitOfWork.Product.AddAsync(viewModel.Product);
            }
            else
            {
                unitOfWork.Product.Update(viewModel.Product);
            }
            
            await unitOfWork.SaveAsync();
            TempData["success"] = "Product added successfully";
            return RedirectToAction("Index");
        }

        var brandList = unitOfWork.Brand
            .GetAllAsQuery()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        var subcategoryList = unitOfWork.Subcategory
            .GetAllAsQuery()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        viewModel.BrandListItems = brandList;
        viewModel.SubcategoryListItems = subcategoryList;
        return View(viewModel);

    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null or 0) return NotFound();

        Product? product = await unitOfWork.Product.GetAsync(o => o.Id == id);
        if (product is null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePOST(int? id)
    {
        Product? product = await unitOfWork.Product.GetAsync(x => x.Id == id);
        if (product is null) return NotFound();

        unitOfWork.Product.Remove(product);
        await unitOfWork.SaveAsync();

        int productsBySubcategory = GetReturnId(product.SubcategoryId);

        TempData["success"] = "Product deleted successfully";

        return RedirectToAction("Index", new { id = productsBySubcategory });
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

    // ref: https://stackoverflow.com/questions/18382311/populating-a-razor-dropdownlist-from-a-listobject-in-mvc
    private int GetReturnId(int productSubcategoryId)
    {
        var subcategory = unitOfWork.Subcategory
            .GetAllAsQuery()
            .Include("Category")
            .FirstOrDefault(p => p.Id == productSubcategoryId);

        return subcategory?.CategoryId ?? 2;
    }
}