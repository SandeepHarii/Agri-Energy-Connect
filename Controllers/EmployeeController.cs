using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;

[Authorize(Roles = "Employee")] // Only users with "Employee" role can access this controller
public class EmployeeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    // Constructor to inject dependencies (UserManager, RoleManager, ApplicationDbContext)
    public EmployeeController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: Employee/Dashboard - Display the Employee dashboard
    public IActionResult Dashboard()
    {
        return View(); // Return the dashboard view
    }

    // GET: Employee/AddFarmer - Display the form to add a new farmer
    public IActionResult AddFarmer()
    {
        return View(); // Return the AddFarmer view
    }

    // POST: Employee/AddFarmer - Handle the form submission to create a new farmer
    [HttpPost]
    [ValidateAntiForgeryToken] // Protect against CSRF attacks
    public async Task<IActionResult> AddFarmer(AddFarmerViewModel model)
    {
        if (ModelState.IsValid) // Check if the model is valid
        {
            // Create a new ApplicationUser (Farmer)
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserType = UserTypeEnum.Farmer // Assign the role as Farmer
            };

            // Create the user with the provided password
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Add the user to the "Farmer" role
                await _userManager.AddToRoleAsync(user, "Farmer");
                TempData["SuccessMessage"] = "Farmer profile created successfully."; // Show success message
                return RedirectToAction("AddFarmer"); // Redirect back to the AddFarmer form
            }

            // If user creation failed, add errors to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model); // Return the view with validation errors if the model is not valid
    }

    // GET: Employee/ViewProducts - Display the products view with optional search filter
    public async Task<IActionResult> ViewProducts(string? searchTerm)
    {
        var query = _context.Products
            .Include(p => p.Farmer) // Include the Farmer details with each product
            .AsQueryable(); // Start with an empty query and add filters

        int intValue;
        string monthFromAbbreviation = null;
        DateTime parsedDate;

        if (!string.IsNullOrEmpty(searchTerm)) // If a search term is provided
        {
            string loweredTerm = searchTerm.ToLower(); // Convert the search term to lowercase

            // Try to convert the search term to a full date
            bool isFullDate = DateTime.TryParse(searchTerm, out parsedDate);

            // Map full month names to their respective numbers (1-12)
            var monthMap = new Dictionary<string, int>
            {
                { "january", 1 }, { "february", 2 }, { "march", 3 }, { "april", 4 },
                { "may", 5 }, { "june", 6 }, { "july", 7 }, { "august", 8 },
                { "september", 9 }, { "october", 10 }, { "november", 11 }, { "december", 12 }
            };

            // Map month abbreviations (e.g., "apr" for "april") to full month names
            var monthAbbreviationMap = new Dictionary<string, string>
            {
                { "jan", "january" }, { "feb", "february" }, { "mar", "march" }, { "apr", "april" },
                { "may", "may" }, { "jun", "june" }, { "jul", "july" }, { "aug", "august" },
                { "sep", "september" }, { "oct", "october" }, { "nov", "november" }, { "dec", "december" }
            };

            // Check if the search term matches any month abbreviation and map it to a full month name
            if (monthAbbreviationMap.TryGetValue(loweredTerm, out string fullMonthName))
            {
                monthFromAbbreviation = fullMonthName;
            }

            // Filter the query based on various search conditions (product name, description, price, date, etc.)
            query = query.Where(p =>
                p.Name.ToLower().Contains(loweredTerm) || // Match product name
                p.Description.ToLower().Contains(loweredTerm) || // Match product description
                (p.Farmer.FirstName + " " + p.Farmer.LastName).ToLower().Contains(loweredTerm) || // Match farmer name
                p.Farmer.FirstName.ToLower().Contains(loweredTerm) || // Match farmer first name
                p.Farmer.LastName.ToLower().Contains(loweredTerm) || // Match farmer last name
                p.Price.ToString().Contains(loweredTerm) || // Match price (as string)
                (int.TryParse(loweredTerm, out intValue) && // Match year, month, or day if numeric
                    (p.ProductionDate.Year == intValue ||
                     p.ProductionDate.Month == intValue ||
                     p.ProductionDate.Day == intValue)) ||

                // Match month abbreviation (e.g., "apr" matches "april")
                (monthFromAbbreviation != null && p.ProductionDate.Month == monthMap[monthFromAbbreviation]) ||

                // Match full date exactly
                (isFullDate && p.ProductionDate.Date == parsedDate.Date)
            );
        }

        // Create the view model to pass the products and search term to the view
        var model = new ProductFilterViewModel
        {
            SearchTerm = searchTerm,
            Products = await query.OrderByDescending(p => p.ProductionDate).ToListAsync() // Order products by production date
        };

        return View(model); // Return the view with the filtered products
    }
}
