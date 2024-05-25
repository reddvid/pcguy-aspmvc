using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Repository;
using PCGuy.Helpers;
using PCGuy.Mvc.Models;

namespace PCGuy.Mvc.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index()
    {
        IEnumerable<Product> products = await unitOfWork.Product.GetAllAsync();
        products = products.Shuffle().Take(10);
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}