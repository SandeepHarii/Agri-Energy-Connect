using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor to inject dependencies (UserManager, SignInManager, RoleManager)
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: /Account/Login - Display the Login page
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Store the return URL in ViewData
            return View(); // Return the login view
        }

        // POST: /Account/Login - Handle the login form submission
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Store the return URL in ViewData

            if (!ModelState.IsValid) // Check if the model is valid
                return View(model); // If not, return to the view with validation errors

            // Attempt to sign in the user with the provided email and password
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email); // Find the user by email
                var roles = await _userManager.GetRolesAsync(user); // Get the user's roles

                // Redirect to the return URL if it’s valid
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                // Redirect based on the user's role
                if (roles.Contains("Farmer"))
                    return RedirectToAction("Dashboard", "Farmer");

                if (roles.Contains("Employee"))
                    return RedirectToAction("Dashboard", "Employee");

                return RedirectToAction("Index", "Home"); // Fallback redirection to Home
            }

            // If the login failed, add an error message to the ModelState
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model); // Return to the login view with an error message
        }

        // GET: /Account/Register - Display the registration page
        [HttpGet]
        public IActionResult Register() => View(); // Return the register view

        // POST: /Account/Register - Handle the registration form submission
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) // Check if the model is valid
                return View(model); // If not, return to the view with validation errors

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserType = model.UserType // Set the user type (Farmer or Employee)
            };

            // Create the user in the system with the provided password
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // If the user is created successfully, assign them to a role based on UserType
                string roleName = model.UserType.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName)) // Check if the role exists
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName)); // Create the role if it doesn't exist
                }

                // Add the user to the assigned role
                var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (roleResult.Succeeded)
                {
                    // Sign the user in and redirect based on their role
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Farmer"))
                        return RedirectToAction("Dashboard", "Farmer");

                    if (roles.Contains("Employee"))
                        return RedirectToAction("Dashboard", "Employee");

                    return RedirectToAction("Index", "Home"); // Fallback redirection to Home
                }

                // If adding the user to the role failed, show errors
                foreach (var error in roleResult.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            // If user creation failed, show the errors
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model); // Return to the registration view with error messages
        }

        // GET: /Account/ForgotPassword - Display the forgot password page
        [HttpGet]
        public IActionResult ForgotPassword() => View(); // Return the forgot password view

        // POST: /Account/ForgotPassword - Handle forgot password logic (currently not implemented)
        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            // TODO: Add password reset logic
            return View(); // Return the forgot password view (with a form to enter email)
        }

        // POST: /Account/Logout - Handle the logout request
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Sign out the user
            return RedirectToAction("Index", "Home"); // Redirect to the home page after logging out
        }
    }
}
