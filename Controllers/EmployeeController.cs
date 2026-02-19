using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
<<<<<<< HEAD

[Authorize(Roles = "Employee")] // Only users with "Employee" role can access this controller
=======
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;

[Authorize(Roles = "Employee")]
>>>>>>> agri-part3/main
public class EmployeeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
<<<<<<< HEAD

    // Constructor to inject dependencies (UserManager, RoleManager, ApplicationDbContext)
    public EmployeeController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
=======
    private readonly IEmailSender _emailSender;

    public EmployeeController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmailSender emailSender)
>>>>>>> agri-part3/main
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
<<<<<<< HEAD
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
=======
        _emailSender = emailSender;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult AddFarmer()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFarmer(AddFarmerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var employeeId = _userManager.GetUserId(User);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            UserType = UserTypeEnum.Farmer,
            RegisteredById = employeeId,
            RegistrationDate = DateTime.Now,
            Status = FarmerStatus.Pending
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Farmer");
            TempData["SuccessMessage"] = "Farmer profile created successfully.";
            return RedirectToAction("AddFarmer");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    public async Task<IActionResult> ViewFarmers()
    {
        var employeeId = _userManager.GetUserId(User);

        var farmers = await _userManager.Users
            .Where(u => u.UserType == UserTypeEnum.Farmer && u.RegisteredById == employeeId)
            .OrderByDescending(u => u.RegistrationDate)
            .Select(u => new FarmerDto
            {
                Id = 0,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status.ToString(),
                Email = u.Email,
                RegistrationDate = u.RegistrationDate
            })
            .ToListAsync();

        return View(new ViewFarmerViewModel { Farmers = farmers });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateFarmer(string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email is required.");

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return NotFound("Farmer not found.");

        user.Status = FarmerStatus.Active;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            TempData["ErrorMessage"] = "Failed to activate the account.";
            return RedirectToAction("ViewFarmers");
        }

        var subject = "Your Farmer Account is Activated";
        var message = $"Hello {user.FirstName},<br/><br/>Your account has been activated. You can now log in and use the system.<br/><br/>Thanks,<br/>Agri Energy Connect Team";

        await _emailSender.SendEmailAsync(user.Email, subject, message);

        TempData["SuccessMessage"] = "Account activated successfully.";
        return RedirectToAction("ViewFarmers");
    }

    // GET: Employee/ViewProducts - Added as requested
    public async Task<IActionResult> ViewProducts(string? searchTerm)
    {
        var query = _context.Products
            .Include(p => p.Farmer)
            .AsQueryable();
>>>>>>> agri-part3/main

        int intValue;
        string monthFromAbbreviation = null;
        DateTime parsedDate;

<<<<<<< HEAD
        if (!string.IsNullOrEmpty(searchTerm)) // If a search term is provided
        {
            string loweredTerm = searchTerm.ToLower(); // Convert the search term to lowercase

            // Try to convert the search term to a full date
            bool isFullDate = DateTime.TryParse(searchTerm, out parsedDate);

            // Map full month names to their respective numbers (1-12)
=======
        if (!string.IsNullOrEmpty(searchTerm))
        {
            string loweredTerm = searchTerm.ToLower();

            bool isFullDate = DateTime.TryParse(searchTerm, out parsedDate);

>>>>>>> agri-part3/main
            var monthMap = new Dictionary<string, int>
            {
                { "january", 1 }, { "february", 2 }, { "march", 3 }, { "april", 4 },
                { "may", 5 }, { "june", 6 }, { "july", 7 }, { "august", 8 },
                { "september", 9 }, { "october", 10 }, { "november", 11 }, { "december", 12 }
            };

<<<<<<< HEAD
            // Map month abbreviations (e.g., "apr" for "april") to full month names
=======
>>>>>>> agri-part3/main
            var monthAbbreviationMap = new Dictionary<string, string>
            {
                { "jan", "january" }, { "feb", "february" }, { "mar", "march" }, { "apr", "april" },
                { "may", "may" }, { "jun", "june" }, { "jul", "july" }, { "aug", "august" },
                { "sep", "september" }, { "oct", "october" }, { "nov", "november" }, { "dec", "december" }
            };

<<<<<<< HEAD
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
=======
            if (monthAbbreviationMap.TryGetValue(loweredTerm, out string fullMonthName))
                monthFromAbbreviation = fullMonthName;

            query = query.Where(p =>
                p.Name.ToLower().Contains(loweredTerm) ||
                p.Description.ToLower().Contains(loweredTerm) ||
                (p.Farmer.FirstName + " " + p.Farmer.LastName).ToLower().Contains(loweredTerm) ||
                p.Farmer.FirstName.ToLower().Contains(loweredTerm) ||
                p.Farmer.LastName.ToLower().Contains(loweredTerm) ||
                p.Price.ToString().Contains(loweredTerm) ||
                (int.TryParse(loweredTerm, out intValue) &&
                    (p.ProductionDate.Year == intValue ||
                     p.ProductionDate.Month == intValue ||
                     p.ProductionDate.Day == intValue)) ||
                (monthFromAbbreviation != null && p.ProductionDate.Month == monthMap[monthFromAbbreviation]) ||
>>>>>>> agri-part3/main
                (isFullDate && p.ProductionDate.Date == parsedDate.Date)
            );
        }

<<<<<<< HEAD
        // Create the view model to pass the products and search term to the view
        var model = new ProductFilterViewModel
        {
            SearchTerm = searchTerm,
            Products = await query.OrderByDescending(p => p.ProductionDate).ToListAsync() // Order products by production date
        };

        return View(model); // Return the view with the filtered products
=======
        var model = new ProductFilterViewModel
        {
            SearchTerm = searchTerm,
            Products = await query.OrderByDescending(p => p.ProductionDate).ToListAsync()
        };

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_ProductTablePartial", model);

        return View(model);
>>>>>>> agri-part3/main
    }
}
