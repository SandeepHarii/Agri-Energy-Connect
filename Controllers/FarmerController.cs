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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            Console.WriteLine("AddProduct POST triggered");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");

                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Validation Error in '{key}': {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            string uniqueFileName = null;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                Console.WriteLine("Image file detected");

                try
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    Console.WriteLine($"Image saved to: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving image: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No image uploaded");
            }

            try
            {
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ImageFileName = uniqueFileName,
                    Category = model.Category,
                    ProductionDate = model.ProductionDate,
                    UserID = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                Console.WriteLine("✅ Product saved to DB");

                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction(nameof(ViewProducts));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Error saving product to DB: {ex.Message}");
                return View(model);
            }
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
                ImageFileName = product.ImageFileName,
                Category = product.Category,
                ProductionDate = product.ProductionDate
            };

            return View(model); // Return the EditProduct view with the product details
        }

        // POST: Farmer/EditProduct/5 - Handle the form submission to update a product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, EditProductViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != model.ProductID)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null || product.UserID != userId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            string uniqueFileName = product.ImageFileName; // keep current image by default

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            }

            // Update product fields
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ImageFileName = uniqueFileName;
            product.Category = model.Category;
            product.ProductionDate = model.ProductionDate;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToAction(nameof(ViewProducts));
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

            if (!string.IsNullOrEmpty(product.ImageFileName))
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var imagePath = Path.Combine(uploadsFolder, product.ImageFileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }


            _context.Products.Remove(product); // Remove the product from the database context
            await _context.SaveChangesAsync(); // Save changes to the database

            TempData["SuccessMessage"] = "Product deleted successfully."; // Show success message
            return RedirectToAction(nameof(ViewProducts)); // Redirect to the ViewProducts action
        }
    }
}
