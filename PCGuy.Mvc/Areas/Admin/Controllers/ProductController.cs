using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Repository;
using PCGuy.Mvc.Models;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
{
    [Route("products/")]
    public async Task<IActionResult> Index()
    {
        var products = await unitOfWork.Product
            .GetAllAsync("Subcategory, Subcategory.Category");

        ViewData["Title"] = "Products";

        return View(products);
    }

    [Route("products/{id:int}")]
    public async Task<IActionResult> Index(int? id)
    {
        var products = await unitOfWork.Product
            .GetAllAsync("Subcategory, Subcategory.Category");

        if (id is null or 0)
        {
            ViewData["Title"] = "All Products";
            return View(products);
        }

        var category = await unitOfWork.Category.GetAsync(o => o.Id == id);
        if (category is null) return View(products);

        var filteredProductsBySubcategory = products.Where(p => p.Subcategory?.Category?.Id == id)
            .ToList();

        ViewData["Title"] = category.Name;
        TempData["ProductSubcategoryId"] = id;

        return View(filteredProductsBySubcategory);
    }

    public async Task<IActionResult> Upsert(int? id)
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

        if (id is null or 0)
        {
            // Create
            return View(productViewModel);
        }

        // Edit
        productViewModel.Product = (await unitOfWork.Product.GetAsync(o => o.Id == id))!;

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

                viewModel.Product.FeaturedImage = @"\images\product\" + fileName;
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

        viewModel.BrandListItems = brandList;
        viewModel.SubcategoryListItems = subcategoryList;
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        var products = await unitOfWork.Product
            .GetAllAsync("Brand,Subcategory");

        var product = products.FirstOrDefault(x => x.Id == id);
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
        var products = await unitOfWork.Product.GetAllAsync("Subcategory.Category");
        return Json(new { data = products });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int? id)
    {
        var product = await unitOfWork.Product.GetAsync(o => o.Id == id);
        if (product is null) return Json(new { success = false, message = "Error while deleting product." });

        if (!string.IsNullOrEmpty(product.FeaturedImage))
        {
            var productImagePath = Path.Combine(webHostEnvironment.WebRootPath,
                product.FeaturedImage.TrimStart('\\'));

            if (System.IO.File.Exists(productImagePath)) System.IO.File.Delete(productImagePath);
        }

        unitOfWork.Product.Remove(product);
        await unitOfWork.SaveAsync();

        return Json(new { success = true, message = "Product deleted." });
    }

    #endregion
}