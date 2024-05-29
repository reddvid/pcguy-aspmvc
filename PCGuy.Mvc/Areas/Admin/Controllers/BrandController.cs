using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.DataAccess.Repository;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.ADMIN)]
public class BrandController(IUnitOfWork unitOfWork) : Controller
{
    // GET
    [Route("brands")]
    public async Task<IActionResult> Index()
    {
        var brands = await unitOfWork.Brand.GetAllAsync();
        brands = brands.OrderBy(a => a.Name);
        return View(brands);
    }

   
}