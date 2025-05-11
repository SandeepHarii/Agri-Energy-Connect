using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Farmer")]
public class FarmerController : Controller
{
    private readonly ApplicationDbContext _context;

    public FarmerController(ApplicationDbContext context)
    {
        _context = context;
    }


    // GET: Farmer/Dashboard

        public IActionResult Dashboard()
        {
            return View();
        }

    // GET: Farmer/AddProduct
    public IActionResult AddProduct()
    {
        return View();
    }

    // POST: Farmer/AddProduct
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    Category = model.Category,
                    ProductionDate = model.ProductionDate,
                    UserID = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };


                _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction(nameof(ViewProducts));
        }

        return View(model);
    }

    // View all products by the logged-in farmer
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> ViewProducts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var products = await _context.Products.Where(p => p.UserID == userId).ToListAsync();
        return View(products);
    }
}

}
