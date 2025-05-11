using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.ViewModels;

namespace AgriEnergyConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserType = model.UserType
            };

            // Create user
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Ensure roles are added if they don't exist
                if (!await _roleManager.RoleExistsAsync(model.UserType.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.UserType.ToString()));
                }

                // Assign the role
                var roleResult = await _userManager.AddToRoleAsync(user, model.UserType.ToString());

                if (roleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in roleResult.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        // POST: /Account/ForgotPassword
        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            // Logic for handling password reset
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
