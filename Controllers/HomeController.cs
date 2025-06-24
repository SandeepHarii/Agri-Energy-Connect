using System.Diagnostics;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Logger for logging errors, info, etc.
        private readonly UserManager<ApplicationUser> _userManager; // UserManager for managing user-related tasks

        // Constructor to inject dependencies (ILogger and UserManager)
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger; // Assign the logger to the private field
            _userManager = userManager; // Assign the UserManager to the private field
        }

        // GET: Home/Index - This is the landing page of the application
        public async Task<IActionResult> Index()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User); // Get the current authenticated user

                // Check if the user has the "Farmer" role
                if (await _userManager.IsInRoleAsync(user, "Farmer"))
                    return RedirectToAction("Dashboard", "Farmer"); // Redirect to Farmer's dashboard

                // Check if the user has the "Employee" role
                if (await _userManager.IsInRoleAsync(user, "Employee"))
                    return RedirectToAction("Dashboard", "Employee"); // Redirect to Employee's dashboard
            }

            // If the user is not authenticated or does not match any of the roles, return the home page view
            return View(); // Anonymous user or role not matched
        }

        // GET: Home/Privacy - Displays the privacy policy page
        public IActionResult Privacy()
        {
            return View(); // Return the Privacy view
        }

        // GET: Home/Error - Displays an error page in case of an error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Disable caching for errors
        public IActionResult Error()
        {
            // Return an error view with a model that contains the request ID (useful for debugging)
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
