using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Repository;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.ADMIN)]
public class CategoryController(IUnitOfWork unitOfWork) : Controller
{
    // GET
    [Route("categories")]
    public async Task<IActionResult> Index()
    {
        var categories = await unitOfWork.Category.GetAllAsync();
        return View(categories);
    }

    public async Task<IActionResult> Upsert(int? productId)
    {
        var category = new Category();
        
        if (productId is null or 0)
        {
            return View(category);
        }
        
        category = await unitOfWork.Category.GetAsync(o => o.Id == productId);
        return View(category);
    }

    [HttpPost, ActionName("Upsert")]
    public async Task<IActionResult> UpsertPOST(Category category)
    {
        if (!ModelState.IsValid) return View();

        await unitOfWork.Category.AddAsync(category);
        await unitOfWork.SaveAsync();
        return RedirectToAction(nameof(Index));
    }


    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await unitOfWork.Category.GetAllAsync();
        return Json(new { data = categories });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int? productId)
    {
        var category = await unitOfWork.Category.GetAsync(o => o.Id == productId);
        if (category is null) return Json(new { success = false, message = "Error while deleting category." });

        unitOfWork.Category.Remove(category);
        await unitOfWork.SaveAsync();

        return Json(new { success = true, message = "Category deleted." });
    }

    #endregion
}