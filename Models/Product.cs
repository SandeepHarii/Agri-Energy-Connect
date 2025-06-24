using AgriEnergyConnect.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Product
{
    // Primary key for the Product entity
    public int ProductID { get; set; }

    // Required Name of the product. This property must be provided for each product
    [Required]
    public string Name { get; set; }

    // Optional Description of the product
    public string Description { get; set; }

    // Price of the product
    public decimal Price { get; set; }

    // Image URL for the product (e.g., a link to an image hosted online)
    public string ImageFileName { get; set; }

    // Required Category of the product. This helps classify products (e.g., "Vegetable", "Fruit")
    [Required]
    public string Category { get; set; }

    // Required ProductionDate to store the date the product was produced
    // The DataType attribute specifies that the field should be treated as a Date
    [Required]
    [DataType(DataType.Date)]
    public DateTime ProductionDate { get; set; }

    // Foreign key to associate the product with a specific user (farmer)
    // This is the UserId that identifies the farmer who listed the product
    [Required]
    public string UserID { get; set; }

    // Relationship with the ApplicationUser entity (Farmer)
    // This property represents the farmer who owns or listed the product
    [ForeignKey("UserID")]
    public ApplicationUser Farmer { get; set; }
}
