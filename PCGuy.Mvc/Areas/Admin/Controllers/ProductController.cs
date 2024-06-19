using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.Models.ViewModels;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.ADMIN)]
public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await unitOfWork.Product
            .GetAllAsync(includeProperties:"Subcategory, Subcategory.Category");

        ViewData["Title"] = "Products";

        return View(products);
    }

    // [Route("products/{id:int}")]
    // public async Task<IActionResult> Index(int? id)
    // {
    //     var products = await unitOfWork.Product
    //         .GetAllAsync(includeProperties: "Subcategory, Subcategory.Category");
    //
    //     if (id is null or 0)
    //     {
    //         ViewData["Title"] = "All Products";
    //         return View(products);
    //     }
    //
    //     var category = await unitOfWork.Category.GetAsync(o => o.Id == id);
    //     if (category is null) return View(products);
    //
    //     var filteredProductsBySubcategory = products.Where(p => p.Subcategory?.Category?.Id == id)
    //         .ToList();
    //
    //     ViewData["Title"] = category.Name;
    //     TempData["ProductSubcategoryId"] = id;
    //
    //     return View(filteredProductsBySubcategory);
    // }

    public async Task<IActionResult> Upsert(int? productId)
    {
        var brandList = (await unitOfWork.Brand
                .GetAllAsync())
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        var subcategoryList = (await unitOfWork.Subcategory
                .GetAllAsync())
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

        if (productId is null or 0)
        {
            // Create
            return View(productViewModel);
        }

        // Edit
        productViewModel.Product = (await unitOfWork.Product.GetAsync(o => o.Id == productId))!;

        return View(productViewModel);
    }

    [HttpPost, ActionName("Upsert")]
    public async Task<IActionResult> UpsertPOST(ProductViewModel productViewModel, List<IFormFile>? files)
    {
        if (ModelState.IsValid)
        {
            if (productViewModel.Product.Id == 0)
                await unitOfWork.Product.AddAsync(productViewModel.Product);
            else
                await unitOfWork.Product.UpdateAsync(productViewModel.Product);

            await unitOfWork.SaveAsync();
            
            var wwwRootPath = webHostEnvironment.WebRootPath;
            if (files is not null && files.Count != 0)
            {
                foreach (IFormFile file in files)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var productPath = $@"images\products\product-{productViewModel.Product.Id}";
                    var productImagesPath = Path.Combine(wwwRootPath, productPath);

                    if (!Directory.Exists(productImagesPath))
                        Directory.CreateDirectory(productImagesPath);
                    
                    await using var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create);
                    await file.CopyToAsync(fileStream);

                    ProductImage productImage = new()
                    {
                        ImageUrl = $@"\{productPath}\{fileName}",
                        ProductId = productViewModel.Product.Id,
                    };

                    productViewModel.Product.ProductImages ??= [];
                    
                    productViewModel.Product.ProductImages.Add(productImage);
                }
                
                await unitOfWork.Product.UpdateAsync(productViewModel.Product);
                await unitOfWork.SaveAsync();

            }
            
            TempData["success"] = "Product added/updated successfully";
            return RedirectToAction(nameof(Index));
        }

        var brandList = (await unitOfWork.Brand
                .GetAllAsync())
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        var subcategoryList = (await unitOfWork.Subcategory
                .GetAllAsync())
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

        productViewModel.BrandListItems = brandList;
        productViewModel.SubcategoryListItems = subcategoryList;
        return View(productViewModel);
    }

    public async Task<IActionResult> Details(int? productId)
    {
        var products = await unitOfWork.Product
            .GetAllAsync(includeProperties: "Brand,Subcategory");

        var product = products.FirstOrDefault(x => x.Id == productId);
        return View(product);
    }

    // ref: https://stackoverflow.com/questions/18382311/populating-a-razor-dropdownlist-from-a-listobject-in-mvc
    private async Task<int> GetReturnIdAsync(int productSubcategoryId)
    {
        var subcategory = await unitOfWork.Subcategory
            .GetAsync(p => p.Id == productSubcategoryId, "Category");

        return subcategory?.CategoryId ?? 2;
    }

    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await unitOfWork.Product.GetAllAsync(includeProperties:"Subcategory.Category");
        return Json(new { data = products });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int? productId)
    {
        var product = await unitOfWork.Product.GetAsync(o => o.Id == productId);
        if (product is null) return Json(new { success = false, message = "Error while deleting product." });

        // if (!string.IsNullOrEmpty(product.FeaturedImage))
        // {
        //     var productImagePath = Path.Combine(webHostEnvironment.WebRootPath,
        //         product.FeaturedImage.TrimStart('\\'));
        //
        //     if (System.IO.File.Exists(productImagePath)) System.IO.File.Delete(productImagePath);
        // }

        unitOfWork.Product.Remove(product);
        await unitOfWork.SaveAsync();

        return Json(new { success = true, message = "Product deleted." });
    }

    #endregion
}