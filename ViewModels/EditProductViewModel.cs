using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    // ViewModel used for editing an existing Product
    public class EditProductViewModel
    {
        // Product ID: Unique identifier for the product (required for identification in the database)
        public int ProductID { get; set; }

        // Product Name: Required field to ensure the product has a name
        [Required]
        public string Name { get; set; }

        // Product Description: Optional field to provide a description of the product
        public string Description { get; set; }

        // Product Price: Must be a positive value, with a validation range
        [Range(0, double.MaxValue)] // Ensures price can't be negative
        public decimal Price { get; set; }

<<<<<<< HEAD
        // Product Image URL: Optional field for a link to an image of the product
        public string ImageUrl { get; set; }
=======
        public IFormFile? ImageFile { get; set; } // for image uploads
        // Product Image URL: Optional field for a link to an image of the product
        public string? ImageFileName { get; set; }
>>>>>>> agri-part3/main

        // Product Category: Optional field to specify the category of the product
        public string Category { get; set; }

        // Production Date: Required field with a date type to indicate when the product was produced
        [DataType(DataType.Date)] // Ensures it's displayed as a date field
        public DateTime ProductionDate { get; set; }
    }
}
