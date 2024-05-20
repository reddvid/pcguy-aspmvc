using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Repository;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
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