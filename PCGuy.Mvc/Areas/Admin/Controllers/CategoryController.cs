using Microsoft.AspNetCore.Mvc;
using PCGuy.Entities.Entities;
using PCGuy.DataAccess.Repository;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController(IUnitOfWork unitOfWork) : Controller
{
    // GET
    [Route("categories")]
    public async Task<IActionResult> Index()
    {
        var categories = await unitOfWork.Category.GetAllAsync();
        return View(categories);
    }

    public async Task<IActionResult> Upsert(int? id)
    {
        var category = new Category();
        
        if (id is null or 0)
        {
            return View(category);
        }
        
        category = await unitOfWork.Category.GetAsync(o => o.Id == id);
        return View(category);
    }

    [HttpPost, ActionName("Upsert")]
    public async Task<IActionResult> CreatePOST(Category category)
    {
        if (!ModelState.IsValid) return View();

        await unitOfWork.Category.AddAsync(category);
        await unitOfWork.SaveAsync();
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null or 0) return NotFound();

        Category? category = await unitOfWork.Category.GetAsync(o => o.Id == id);
        if (category is null) return NotFound();
        
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePOST(int? id)
    {
        Category? category = await unitOfWork.Category.GetAsync(o => o.Id == id);
        if (category is null) return NotFound();

        unitOfWork.Category.Remove(category);
        
        return RedirectToAction("Index");
    }
}