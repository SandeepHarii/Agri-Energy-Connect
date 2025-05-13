using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    public class EditProductViewModel
    {
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
    }

}
