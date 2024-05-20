using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Repository;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
public class SubcategoryController(IUnitOfWork unitOfWork) : Controller
{
    [Route("subcategories")]
    public async Task<IActionResult> Index()
    {
        var subcategories = await unitOfWork.Subcategory.GetAllAsync();
        subcategories = subcategories.OrderBy(a => a.Name);
        return View(subcategories);
    }

   
}