using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        public int FarmerID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Location { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public List<Product> Products { get; set; }
    }

}
