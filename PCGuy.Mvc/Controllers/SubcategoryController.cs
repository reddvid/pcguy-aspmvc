using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Data;
using PCGuy.DataAccess.Repository;

namespace PCGuy.Mvc.Controllers;

public class SubcategoryController(IUnitOfWork unitOfWork) : Controller
{
    [Route("subcategories")]
    public async Task<IActionResult> Index()
    {
        var subcategories = await unitOfWork.Subcategory.GetAll();
        subcategories = subcategories.OrderBy(a => a.Name);
        return View(subcategories);
    }

   
}