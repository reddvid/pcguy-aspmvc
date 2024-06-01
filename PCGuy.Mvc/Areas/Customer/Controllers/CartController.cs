using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Models.ViewModels;

namespace PCGuy.Mvc.Areas.Customer.Controllers;

[Area(nameof(Customer))]
[Authorize]
public class CartController(IUnitOfWork unitOfWork) : Controller
{
    public required ShoppingCartViewModel CartViewModel { get; set; }
    // GET
    public async Task<IActionResult> Index()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        CartViewModel = new ShoppingCartViewModel
        {
            ShoppingCartList = await unitOfWork.ShoppingCart.GetAllAsync(o => o.ApplicationUserId == userId, "Product")
        };

        foreach (var cart in CartViewModel.ShoppingCartList)
        {
            CartViewModel.OrderTotal += cart.Product.Price * cart.Count;
        }

        return View(CartViewModel);
    }
    
}