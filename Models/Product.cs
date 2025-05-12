using AgriEnergyConnect.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int ProductID { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }

    [Required]
    public string Category { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime ProductionDate { get; set; }

    [Required]
    public string UserID { get; set; }

    [ForeignKey("UserID")]
    public ApplicationUser Farmer { get; set; }
}
