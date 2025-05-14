using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    // ViewModel for creating or editing product information
    public class ProductViewModel
    {
        // Name: The name of the product. This is a required field, and its length must be between 3 and 100 characters.
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        // Description: A required field that provides a detailed description of the product.
        [Required]
        public string Description { get; set; }

        // Price: The price of the product. The value must be greater than 0.
        // If the user enters a price less than or equal to 0, an error message is displayed.
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        // ImageUrl: An optional field that allows the user to provide a URL to an image of the product.
        public string ImageUrl { get; set; } // Optional image upload

        // Category: The category to which the product belongs. This is a required field.
        [Required]
        public string Category { get; set; }

        // ProductionDate: The date the product was produced. This is a required field, and its value should be in date format.
        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
    }
}
