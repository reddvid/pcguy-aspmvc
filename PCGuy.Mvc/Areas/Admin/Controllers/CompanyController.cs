using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCGuy.DataAccess.Contracts;
using PCGuy.Models.Entities;
using PCGuy.DataAccess.Repository;
using PCGuy.Models.ViewModels;
using PCGuy.Helpers;

namespace PCGuy.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.ADMIN)]
public class CompanyController(IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index()
    {
        var companies = await unitOfWork.Company.GetAllAsync();

        ViewData["Title"] = "Companies";

        return View(companies);
    }

    public async Task<IActionResult> Upsert(int? productId)
    {
        if (productId is null or 0)
        {
            // Create
            return View(new Company());
        }

        // Edit
        Company company = (await unitOfWork.Company.GetAsync(o => o.Id == productId))!;

        return View(company);
    }

    [HttpPost, ActionName("Upsert")]
    public async Task<IActionResult> UpsertPOST(Company company)
    {
        if (!ModelState.IsValid) return View(company);
        
        if (company.Id == 0)
        {
            await unitOfWork.Company.AddAsync(company);
        }
        else
        {
            unitOfWork.Company.Update(company);
        }

        await unitOfWork.SaveAsync();
        TempData["success"] = "Company added successfully";
        return RedirectToAction(nameof(Index));

    }

    // public async Task<IActionResult> Details(int? id)
    // {
    //     var products = await unitOfWork.Company
    //         .GetAllAsync("Brand,Subcategory");
    //
    //     var product = products.FirstOrDefault(x => x.Id == id);
    //     return View(product);
    // }

    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var companies = await unitOfWork.Company.GetAllAsync();
        return Json(new { data = companies });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int? productId)
    {
        var company = await unitOfWork.Company.GetAsync(o => o.Id == productId);
        if (company is null) return Json(new { success = false, message = "Error while deleting company." });
        
        unitOfWork.Company.Remove(company);
        await unitOfWork.SaveAsync();

        return Json(new { success = true, message = "Company deleted." });
    }

    #endregion
}