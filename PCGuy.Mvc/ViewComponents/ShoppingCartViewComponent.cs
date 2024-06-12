using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PCGuy.DataAccess.Contracts;
using PCGuy.Helpers;

namespace PCGuy.Mvc.ViewComponents;

public class ShoppingCartViewComponent(IUnitOfWork unitOfWork) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim is not null)
        {
            if (HttpContext.Session.GetInt32(SessionKeys.Cart) is null)
            {
                HttpContext.Session.SetInt32(SessionKeys.Cart, (await unitOfWork.ShoppingCart.GetAllAsync(o => o.ApplicationUserId == claim.Value)).Count());
            }
            
            return View(HttpContext.Session.GetInt32(SessionKeys.Cart));
        }

        HttpContext.Session.Clear();
        return View(0);
    } 
}