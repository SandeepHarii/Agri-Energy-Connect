using AgriEnergyConnect.Models;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    // ViewModel for the Login functionality
    public class LoginViewModel
    {
        // Email address field, required and must follow the email format
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Password field, required and should be treated as a password input
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Boolean flag to remember the user on subsequent visits (for persistent login)
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    // ViewModel for the Register functionality
    public class RegisterViewModel
    {
        // First name field, required
        [Required]
        public string FirstName { get; set; }

        // Last name field, required
        [Required]
        public string LastName { get; set; }

        // Phone number field, required
        [Required]
        public string PhoneNumber { get; set; }

        // Email address field, required and must follow the email format
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Password field, required and treated as a password input
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Confirm password field, used to ensure the user types the correct password
        // It must match the "Password" field, specified by the [Compare] attribute
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // User type field, which defines whether the user is a Farmer, Employee, etc.
        // This is now an enum for better control and validation
        [Required]
        public UserTypeEnum UserType { get; set; }

        // Optional return URL, typically used to redirect the user back to the previous page after successful registration/login
        public string ReturnUrl { get; set; }
    }
}
