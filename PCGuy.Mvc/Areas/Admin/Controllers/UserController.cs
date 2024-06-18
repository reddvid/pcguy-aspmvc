using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PCGuy.DataAccess.Contracts;
using PCGuy.DataAccess.Data;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Repository;
using PCGuy.Models.ViewModels;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.ADMIN)]
public class UserController(ApplicationDbContext db) : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Manage Users";
        return View();
    }

    public async Task<IActionResult> Manage(string? userId)
    {
        ViewData["Title"] = "Manage User Permissions";

        string roleId = (await db.UserRoles.FirstOrDefaultAsync(o => o.UserId == userId))!.RoleId;
        
        var roles = await db.Roles.ToListAsync();
        var companies = await db.Companies.ToListAsync();

        var manageUserViewModel = new ManageUserViewModel
        {
            ApplicationUser = await db.ApplicationUsers.Include(o => o.Company).FirstOrDefaultAsync(o => o.Id == userId),
            CompanyList = companies.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            RolesList = roles.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            })
        };

        manageUserViewModel.ApplicationUser!.Role = (await db.Roles.FirstOrDefaultAsync(o => o.Id == roleId))?.Name;
        
        return View(manageUserViewModel);
    }


    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await db.ApplicationUsers.Include(o => o.Company).ToListAsync();
        var userRoles = await db.UserRoles.ToListAsync();
        var roles = await db.Roles.ToListAsync();

        foreach (var user in users)
        {
            var roleId = userRoles.FirstOrDefault(o => o.UserId == user.Id)?.RoleId;
            user.Role = roles.FirstOrDefault(o => o.Id == roleId)?.Name;

            user.Company ??= new Company { Name = string.Empty };
        }

        return Json(new { data = users });
    }

    [HttpPost]
    public async Task<IActionResult> LockUnlock([FromBody] string? id)
    {
        var user = await db.ApplicationUsers.FirstOrDefaultAsync(o => o.Id == id);
        string lockUnlock;

        if (user is null)
        {
            return Json(new { success = true, message = "Something went wrong while locking/unlocking user." });
        }

        if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
        {
            // Unlock
            user.LockoutEnd = DateTime.Now;
            lockUnlock = "unlocked";
        }
        else
        {
            user.LockoutEnd = DateTime.Now.AddDays(30);
            lockUnlock = "locked";
        }

        await db.SaveChangesAsync();

        return Json(new { success = true, message = $"User is now {lockUnlock}" });
    }

    #endregion
}