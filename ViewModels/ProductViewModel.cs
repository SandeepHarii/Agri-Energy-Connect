using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } // Optional image upload

        [Required]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
    }

}
