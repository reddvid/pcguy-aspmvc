using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Helpers;
using PCGuy.Models.Entities;
using PCGuy.Models.ViewModels;
using Stripe;
using Stripe.Climate;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OrderController(IUnitOfWork unitOfWork) : Controller
{
    [BindProperty] public required OrderViewModel OrderViewModel { get; set; }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Details(int orderId)
    {
        OrderViewModel = new OrderViewModel
        {
            OrderHeader = (await unitOfWork.OrderHeader.GetAsync(o => o.Id == orderId, "ApplicationUser"))!,
            OrderDetails = await unitOfWork.OrderDetail.GetAllAsync(o => o.OrderHeaderId == orderId, "Product"),
        };

        return View(OrderViewModel);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.EMPLOYEE}")]
    public async Task<IActionResult> UpdateOrderDetails(int orderId)
    {
        var orderHeader = await unitOfWork.OrderHeader.GetAsync(o => o.Id == OrderViewModel.OrderHeader.Id);

        if (orderHeader is null) return NotFound();

        orderHeader.Name = OrderViewModel.OrderHeader.Name;
        orderHeader.PhoneNumber = OrderViewModel.OrderHeader.PhoneNumber;
        orderHeader.StreetAddress = OrderViewModel.OrderHeader.StreetAddress;
        orderHeader.City = OrderViewModel.OrderHeader.City;
        orderHeader.State = OrderViewModel.OrderHeader.State;
        orderHeader.PostalCode = OrderViewModel.OrderHeader.PostalCode;

        if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.Carrier))
        {
            orderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
        }

        if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.TrackingNumber))
        {
            orderHeader.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
        }

        unitOfWork.OrderHeader.Update(orderHeader);
        await unitOfWork.SaveAsync();

        TempData["success"] = "Order Details Updated Successfully";

        return RedirectToAction(nameof(Details), new { orderId = orderHeader.Id });
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.EMPLOYEE}")]
    public async Task<IActionResult> ProcessOrder()
    {
        await unitOfWork.OrderHeader.UpdateStatusAsync(OrderViewModel.OrderHeader.Id, OrderStatus.PROCESSING);
        await unitOfWork.SaveAsync();

        TempData["success"] = "Order Is Now Being Processed";

        return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id });
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.EMPLOYEE}")]
    public async Task<IActionResult> ShipOrder()
    {
        var orderHeader = await unitOfWork.OrderHeader.GetAsync(o => o.Id == OrderViewModel.OrderHeader.Id);

        if (orderHeader is null) return NotFound();

        orderHeader.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
        orderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
        orderHeader.OrderStatus = OrderStatus.SHIPPED;
        orderHeader.ShippingDate = DateTime.Now;

        if (orderHeader.PaymentStatus == PaymentStatus.DELAYED_PAYMENT)
        {
            orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
        }

        unitOfWork.OrderHeader.Update(orderHeader);
        await unitOfWork.SaveAsync();

        TempData["success"] = "Order Shipped Successfully";

        return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id });
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.EMPLOYEE}")]
    public async Task<IActionResult> CancelOrder()
    {
        var orderHeader = await unitOfWork.OrderHeader.GetAsync(o => o.Id == OrderViewModel.OrderHeader.Id);

        if (orderHeader is null) return NotFound();

        if (orderHeader.PaymentStatus == PaymentStatus.APPROVED)
        {
            var options = new RefundCreateOptions()
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeader.PaymentIntentId,
            };

            var service = new RefundService();
            Refund refund = await service.CreateAsync(options);

            await unitOfWork.OrderHeader.UpdateStatusAsync(orderHeader.Id, OrderStatus.CANCELLED,
                PaymentStatus.REFUNDED);
        }
        else
        {
            await unitOfWork.OrderHeader.UpdateStatusAsync(orderHeader.Id, OrderStatus.CANCELLED,
                PaymentStatus.CANCELLED);
        }

        await unitOfWork.SaveAsync();

        TempData["success"] = "Order was Cancelled";

        return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id });
    }


    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetAll(string? status)
    {
        IEnumerable<OrderHeader> orders;

        if (User.IsInRole(Roles.ADMIN) || User.IsInRole(Roles.EMPLOYEE))
        {
            orders = await unitOfWork.OrderHeader.GetAllAsync(null, "ApplicationUser");
        }
        else
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            orders = await unitOfWork.OrderHeader.GetAllAsync(o => o.ApplicationUserId == userId, "ApplicationUser");
        }

        switch (status)
        {
            case "processing":
                orders = orders.Where(o => o.PaymentStatus == PaymentStatus.DELAYED_PAYMENT);
                break;
            case "pending":
                orders = orders.Where(o => o.OrderStatus == OrderStatus.PROCESSING);
                break;
            case "completed":
                orders = orders.Where(o => o.OrderStatus == OrderStatus.SHIPPED);
                break;
            case "approved":
                orders = orders.Where(o => o.OrderStatus == OrderStatus.APPROVED);
                break;
        }

        return Json(new { data = orders });
    }

    #endregion
}