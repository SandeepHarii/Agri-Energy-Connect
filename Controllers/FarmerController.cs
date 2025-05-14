using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Farmer")] // Only users with "Farmer" role can access this controller
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor to inject dependencies (ApplicationDbContext and UserManager)
        public FarmerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Farmer/Dashboard - Display the Farmer's dashboard
        public IActionResult Dashboard()
        {
            return View(); // Return the dashboard view
        }

        // GET: Farmer/AddProduct - Display the form to add a new product
        public IActionResult AddProduct()
        {
            return View(); // Return the AddProduct view
        }

        // POST: Farmer/AddProduct - Handle the form submission to add a new product
        [HttpPost]
        [ValidateAntiForgeryToken] // Protect against CSRF attacks
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid) // Check if the model is valid
            {
                // Create a new Product object with the values from the view model
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    Category = model.Category,
                    ProductionDate = model.ProductionDate,
                    UserID = User.FindFirstValue(ClaimTypes.NameIdentifier) // Get the current user's ID
                };

                _context.Add(product); // Add the product to the database context
                await _context.SaveChangesAsync(); // Save the changes to the database

                TempData["SuccessMessage"] = "Product added successfully!"; // Show success message
                return RedirectToAction(nameof(ViewProducts)); // Redirect to the ViewProducts action
            }

            return View(model); // If the model is invalid, return the view with errors
        }

        // GET: Farmer/ViewProducts - Display all products added by the current farmer
        public async Task<IActionResult> ViewProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            var products = await _context.Products
                .Where(p => p.UserID == userId) // Only fetch products added by the current farmer
                .ToListAsync(); // Asynchronously fetch products from the database

            return View(products); // Return the ViewProducts view with the list of products
        }

        // GET: Farmer/EditProduct/5 - Display the form to edit a product
        public async Task<IActionResult> EditProduct(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            var product = await _context.Products.FindAsync(id); // Find the product by ID

            // If the product doesn't exist or isn't owned by the current user, return a 404 error
            if (product == null || product.UserID != userId)
            {
                return NotFound();
            }

            // Populate the EditProductViewModel with the product's current details
            var model = new EditProductViewModel
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Category = product.Category,
                ProductionDate = product.ProductionDate
            };

            return View(model); // Return the EditProduct view with the product details
        }

        // POST: Farmer/EditProduct/5 - Handle the form submission to update a product
        [HttpPost]
        [ValidateAntiForgeryToken] // Protect against CSRF attacks
        public async Task<IActionResult> EditProduct(int id, EditProductViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID

            if (id != model.ProductID) // Ensure the ID from the URL matches the ID in the model
                return NotFound(); // Return 404 if they don't match

            var product = await _context.Products.FindAsync(id); // Find the product by ID
            if (product == null || product.UserID != userId) // Check if the product exists and belongs to the current user
                return NotFound(); // Return 404 if not found or unauthorized access

            if (!ModelState.IsValid) // If the model is not valid
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage); // Log validation errors for debugging
                }

                return View(model); // Return the view with validation errors
            }

            // Update the product with the new values from the form
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ImageUrl = model.ImageUrl;
            product.Category = model.Category;
            product.ProductionDate = model.ProductionDate;

            await _context.SaveChangesAsync(); // Save the updated product to the database

            TempData["SuccessMessage"] = "Product updated successfully."; // Show success message
            return RedirectToAction(nameof(ViewProducts)); // Redirect to the ViewProducts action
        }

        // GET: Farmer/DeleteProduct/5 - Display the confirmation for deleting a product
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            var product = await _context.Products.FindAsync(id); // Find the product by ID

            // If the product doesn't exist or isn't owned by the current user, return a 404 error
            if (product == null || product.UserID != userId)
            {
                return NotFound();
            }

            _context.Products.Remove(product); // Remove the product from the database context
            await _context.SaveChangesAsync(); // Save changes to the database

            TempData["SuccessMessage"] = "Product deleted successfully."; // Show success message
            return RedirectToAction(nameof(ViewProducts)); // Redirect to the ViewProducts action
        }
    }
}
