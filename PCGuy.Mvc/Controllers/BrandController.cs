using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Data;
using PCGuy.DataAccess.Repository;

namespace PCGuy.Mvc.Controllers;

public class BrandController(IUnitOfWork unitOfWork) : Controller
{
    // GET
    [Route("brands")]
    public async Task<IActionResult> Index()
    {
        var brands = await unitOfWork.Brand.GetAll();
        brands = brands.OrderBy(a => a.Name);
        return View(brands);
    }

   
}