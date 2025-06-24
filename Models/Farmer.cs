using AgriEnergyConnect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Farmer
{
    // Primary key for the Farmer entity
    public int FarmerID { get; set; }

    // Required Name of the farmer. This property must be provided for each farmer
    [Required]
    public string Name { get; set; }

    // Required Contact number of the farmer. This property must be provided for each farmer
    [Required]
    public string Contact { get; set; }

    // Required Location where the farmer operates. This property must be provided for each farmer
    [Required]
    public string Location { get; set; }

    // Foreign key to link the Farmer to a User
    // This property stores the User's ID that corresponds to this farmer
    [Required]
    public string UserId { get; set; }

    // Relationship with the ApplicationUser entity
    // This property represents the ApplicationUser linked to the Farmer, 
    // which can be a Farmer or Employee (but in this case, it’s likely a Farmer)
    [ForeignKey("UserId")] // Explicitly define the foreign key for this relationship
    public ApplicationUser User { get; set; }

    // Navigation property for the related Product entities
    // This property represents a list of products the farmer has listed on the platform
    public List<Product> Products { get; set; }

    // New: Status of the farmer account, default is Pending
    public FarmerStatus Status { get; set; } = FarmerStatus.Pending;

    // New: Date the farmer was registered (optional but useful)
    public DateTime DateRegistered { get; set; } = DateTime.Now;
}
