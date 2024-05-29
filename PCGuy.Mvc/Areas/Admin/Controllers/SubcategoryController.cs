using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.DataAccess.Repository;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.ADMIN)]
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