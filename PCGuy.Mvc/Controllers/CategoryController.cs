using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Data;

namespace PCGuy.Mvc.Controllers;

public class CategoryController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET
    [Route("categories")]
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }

   
}