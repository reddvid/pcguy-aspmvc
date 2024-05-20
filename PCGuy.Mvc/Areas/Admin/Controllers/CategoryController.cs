using Microsoft.AspNetCore.Mvc;
using PCGuy.Common.Entities;
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

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, ActionName("Create")]
    public async Task<IActionResult> CreatePOST(Category category)
    {
        if (!ModelState.IsValid) return View();

        await unitOfWork.Category.AddAsync(category);
        await unitOfWork.SaveAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null or 0) return NotFound();

        Category? category = await unitOfWork.Category.GetAsync(o => o.Id == id);
        if (category is null) return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Edit")]
    public async Task<IActionResult> EditPOST(Category category)
    {
        if (!ModelState.IsValid) return View();

        unitOfWork.Category.Update(category);
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