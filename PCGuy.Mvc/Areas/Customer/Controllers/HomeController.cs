using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Repository;
using PCGuy.Models.ViewModels;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Customer.Controllers;

[Area(nameof(Customer))]
public class HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        
        if (claim is not null)
        {
            HttpContext.Session.SetInt32(Sessions.Cart, (await unitOfWork.ShoppingCart.GetAllAsync(o =>
                o.ApplicationUserId == claim.Value)).Count());
        }
        
        IEnumerable<Product> products = await unitOfWork.Product.GetAllAsync();
        products = products.Shuffle().Take(10);
        return View(products);
    }

    public async Task<IActionResult> Details(int productId)
    {
        Product? product =
            await unitOfWork.Product.GetAsync(o => o.Id == productId, "Subcategory, Subcategory.Category");

        ShoppingCart cart = new()
        {
            Product = product!,
            Count = 1,
            ProductId = productId
        };

        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Details(ShoppingCart cart)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        cart.ApplicationUserId = userId!;

        ShoppingCart? cartFromDb = await unitOfWork.ShoppingCart.GetAsync(o =>
            o.ApplicationUserId == userId &&
            o.ProductId == cart.ProductId);

        if (cartFromDb is not null)
        {
            cartFromDb.Count += cart.Count;
            unitOfWork.ShoppingCart.Update(cartFromDb);
            await unitOfWork.SaveAsync();
        }
        else // Add Cart
        {
            await unitOfWork.ShoppingCart.AddAsync(cart);
            await unitOfWork.SaveAsync();
            
            HttpContext.Session.SetInt32(Sessions.Cart, (await unitOfWork.ShoppingCart.GetAllAsync(o =>
                o.ApplicationUserId == userId)).Count());
        }

        TempData["success"] = "Cart updated";

        return RedirectToAction(nameof(Index));
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