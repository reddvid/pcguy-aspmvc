using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PCGuy.Common.Entities;
using PCGuy.Mvc.Data;

namespace PCGuy.Mvc.Controllers;

public class ProductController(ApplicationDbContext context) : Controller
{
    // GET
    [Route("/products/{id:int}")]
    public async Task<IActionResult> Index([FromRoute] int id)
    {
        Console.WriteLine("ID" + id);
        var peripherals = await context.Products
            .Where(p => p.SubCategory!.Category!.Id == id)
            .ToListAsync();

        ViewData["Title"] = "Categories: " + id switch
        {
            1 => "Software",
            2 => "PC Parts",
            3 => "Peripherals",
            4 => "Accessories",
            _ => ViewData["Title"]
        };

        return View(peripherals);
    }
}