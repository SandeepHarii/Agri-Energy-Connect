using AgriEnergyConnect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Farmer
{
    public int FarmerID { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Contact { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")] //explicitly define foreign key
    public ApplicationUser User { get; set; }

    public List<Product> Products { get; set; }
}
