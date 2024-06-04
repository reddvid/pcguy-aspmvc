using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Helpers;
using PCGuy.Models.Entities;
using PCGuy.Models.ViewModels;
using Stripe.Checkout;
using Stripe.FinancialConnections;
using Session = Stripe.Checkout.Session;
using SessionService = Stripe.FinancialConnections.SessionService;

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

        var shoppingCarts = CartViewModel.ShoppingCartList.ToList();
        var shoppingCartList = shoppingCarts.ToList();

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
            var domain = "https://localhost:7208/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = $"{domain}customer/cart/OrderConfirmation?id={CartViewModel.OrderHeader.Id}",
                CancelUrl = $"{domain}customer/cart/index",
                LineItems = [],
                Mode = "payment",
            };

            foreach (var item in shoppingCarts)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "PHP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Count,
                };
                
                options.LineItems.Add(sessionLineItem);
            }
            
            var service = new Stripe.Checkout.SessionService();
            var session = await service.CreateAsync(options);
            
            await unitOfWork.OrderHeader.UpdateStripePaymentIdAsync(CartViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
            await unitOfWork.SaveAsync();
            
            Response.Headers["Location"] = session.Url;
            return new StatusCodeResult(303);
        }
        else // Company User
        {
        }

        return RedirectToAction(nameof(OrderConfirmation), new { id = CartViewModel.OrderHeader.Id });
    }

    public async Task<IActionResult> OrderConfirmation(int id)
    {
        OrderHeader? orderHeader = await unitOfWork.OrderHeader.GetAsync(o => o.Id == id, "ApplicationUser");

        if (orderHeader is null) return NotFound();

        if (orderHeader.PaymentStatus != PaymentStatus.DELAYED_PAYMENT)
        {
            var service = new Stripe.Checkout.SessionService();
            var session = await service.GetAsync(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                await unitOfWork.OrderHeader.UpdateStripePaymentIdAsync(id, session.Id, session.PaymentIntentId);
                await unitOfWork.OrderHeader.UpdateStatusAsync(id, OrderStatus.APPROVED, PaymentStatus.APPROVED);
                await unitOfWork.SaveAsync();
            }
        }

        var carts = await unitOfWork.ShoppingCart.GetAllAsync(o =>
            o.ApplicationUserId == orderHeader.ApplicationUserId);
        
        unitOfWork.ShoppingCart.RemoveRange(carts);

        await unitOfWork.SaveAsync();
        
        return View(id);
    }
}