using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Data;
using PCGuy.DataAccess.Repository;

namespace PCGuy.Mvc.Controllers;

public class CategoryController(IUnitOfWork unitOfWork) : Controller
{
    // GET
    [Route("categories")]
    public async Task<IActionResult> Index()
    {
        var categories = await unitOfWork.Category.GetAll();
        return View(categories);
    }

   
}