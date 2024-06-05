using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.ADMIN)]
public class OrderController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index()
    {
        return View();
    }


    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetAll(string? status)
    {
        var orders = await unitOfWork.OrderHeader.GetAllAsync(null, "ApplicationUser");
        
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