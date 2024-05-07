using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PCGuy.Common.Entities;
using PCGuy.DataAccess.Data;
using PCGuy.DataAccess.Repository;

namespace PCGuy.Mvc.Controllers;

public class CategoryController(ICategoryRepository categoryRepository) : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    
    // GET
    [Route("categories")]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepository.GetAll();
        return View(categories);
    }

   
}