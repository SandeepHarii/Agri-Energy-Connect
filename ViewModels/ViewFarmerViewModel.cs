using System;
using System.Collections.Generic;

namespace AgriEnergyConnect.ViewModels
{
    public class ViewFarmerViewModel
    {
        public List<FarmerDto> Farmers { get; set; } = new List<FarmerDto>();
    }

    public class FarmerDto
    {
        public int Id { get; set; }  // Not used here, but can keep it
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }  // Pending or Active
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
