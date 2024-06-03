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
        var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        CartViewModel = new ShoppingCartViewModel
        {
            ShoppingCartList = await unitOfWork.ShoppingCart.GetAllAsync(o => o.ApplicationUserId == userId, "Product"),
            OrderHeader = new()
        };

        foreach (var cart in CartViewModel.ShoppingCartList)
        {
            CartViewModel.OrderHeader!.OrderTotal += cart.Product.Price * cart.Count;
        }

        return View(CartViewModel);
    }

    public async Task<IActionResult> Increment(int cartId)
    {
        var cartFromDb = await unitOfWork.ShoppingCart.GetAsync(o => o.Id == cartId);
        cartFromDb!.Count += 1;
        
        unitOfWork.ShoppingCart.Update(cartFromDb);
        await unitOfWork.SaveAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Decrement(int cartId)
    {
        var cartFromDb = await unitOfWork.ShoppingCart.GetAsync(o => o.Id == cartId);

        if (cartFromDb!.Count <= 1)
        {
            // Remove from cart
            unitOfWork.ShoppingCart.Remove(cartFromDb);
        }
        else
        {
            cartFromDb.Count -= 1;
            unitOfWork.ShoppingCart.Update(cartFromDb);
        }

        await unitOfWork.SaveAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Remove(int cartId)
    {
        var cartFromDb = await unitOfWork.ShoppingCart.GetAsync(o => o.Id == cartId);
        
        unitOfWork.ShoppingCart.Remove(cartFromDb!);
        await unitOfWork.SaveAsync();
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Summary()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        CartViewModel = new ShoppingCartViewModel
        {
            ShoppingCartList = await unitOfWork.ShoppingCart.GetAllAsync(o => o.ApplicationUserId == userId, "Product"),
            OrderHeader = new()
        };

        CartViewModel.OrderHeader.ApplicationUser = await unitOfWork.ApplicationUser.GetAsync(o => o.Id == userId);
        CartViewModel.OrderHeader.Name = CartViewModel.OrderHeader.ApplicationUser!.Name;
        CartViewModel.OrderHeader.PhoneNumber = CartViewModel.OrderHeader.ApplicationUser!.PhoneNumber;
        CartViewModel.OrderHeader.StreetAddress = CartViewModel.OrderHeader.ApplicationUser!.StreetAddress;
        CartViewModel.OrderHeader.City = CartViewModel.OrderHeader.ApplicationUser!.City;
        CartViewModel.OrderHeader.State = CartViewModel.OrderHeader.ApplicationUser!.State;
        CartViewModel.OrderHeader.PostalCode = CartViewModel.OrderHeader.ApplicationUser!.PostalCode;
        
        foreach (var cart in CartViewModel.ShoppingCartList)
        {
            CartViewModel.OrderHeader.OrderTotal += cart.Product.Price * cart.Count;
        }
        
        return View(CartViewModel);
    }
}