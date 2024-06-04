using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Helpers;
using PCGuy.Models.Entities;
using PCGuy.Models.ViewModels;

namespace PCGuy.Mvc.Areas.Customer.Controllers;

[Area(nameof(Customer))]
[Authorize]
public class CartController(IUnitOfWork unitOfWork) : Controller
{
    [BindProperty] public required ShoppingCartViewModel CartViewModel { get; set; }


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

    [HttpPost]
    [ActionName("Summary")]
    public async Task<IActionResult> SummaryPOST()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        CartViewModel.ShoppingCartList =
            await unitOfWork.ShoppingCart.GetAllAsync(o => o.ApplicationUserId == userId, "Product");

        CartViewModel.OrderHeader.OrderDate = DateTime.Now;
        CartViewModel.OrderHeader.ApplicationUserId = userId;
        ApplicationUser? appUser = await unitOfWork.ApplicationUser.GetAsync(o => o.Id == userId);

        var shoppingCartList = CartViewModel.ShoppingCartList.ToList();

        foreach (var cart in shoppingCartList)
        {
            CartViewModel.OrderHeader.OrderTotal += cart.Product.Price * cart.Count;
        }

        if (appUser?.CompanyId.GetValueOrDefault() == 0) // Regular Customer Account
        {
            CartViewModel.OrderHeader.PaymentStatus = PaymentStatus.PENDING;
            CartViewModel.OrderHeader.OrderStatus = OrderStatus.PENDING;
        }
        else // Company User
        {
            CartViewModel.OrderHeader.PaymentStatus = PaymentStatus.DELAYED_PAYMENT;
            CartViewModel.OrderHeader.OrderStatus = OrderStatus.APPROVED;
        }

        await unitOfWork.OrderHeader.AddAsync(CartViewModel.OrderHeader);
        await unitOfWork.SaveAsync();

        foreach (var orderDetail in shoppingCartList.Select(
                     cart => new OrderDetail
                     {
                         ProductId = cart.ProductId,
                         OrderHeaderId = CartViewModel.OrderHeader.Id,
                         Price = cart.Product.Price,
                         Count = cart.Count,
                     })
                )
        {
            await unitOfWork.OrderDetail.AddAsync(orderDetail);
            await unitOfWork.SaveAsync();
        }

        if (appUser?.CompanyId.GetValueOrDefault() == 0) // Regular Customer Account
        {
        }
        else // Company User
        {
        }

        return RedirectToAction(nameof(OrderConfirmation), new { id = CartViewModel.OrderHeader.Id });
    }

    public IActionResult OrderConfirmation(int id)
    {
        return View(id);
    }
}