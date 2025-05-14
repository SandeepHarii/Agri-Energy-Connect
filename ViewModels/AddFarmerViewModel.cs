using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    // ViewModel used for adding a new Farmer
    public class AddFarmerViewModel
    {
        // First Name: Required field, and custom display name for the view
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // Last Name: Required field, and custom display name for the view
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Email Address: Required field, email must be in a valid email format
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        // Phone Number: Required field, and phone number must be valid
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        // Temporary Password: Required field for assigning a temporary password to the Farmer
        // This is stored as a plain text password and should be handled securely
        [Required]
        [DataType(DataType.Password)] // Ensures password is treated as a password field
        [Display(Name = "Temporary Password")]
        public string Password { get; set; }
    }
}
