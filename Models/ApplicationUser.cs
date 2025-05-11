using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AgriEnergyConnect.Models
{
    public enum UserTypeEnum
    {
        Farmer,
        Employee
        
    }

    public class ApplicationUser : IdentityUser
    {
        public UserTypeEnum UserType { get; set; } // Changed to enum for better control
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Access flags
        public bool HasMarketplaceAccess { get; set; }
        public bool HasForumAccess { get; set; }
        public bool HasProductListingAccess { get; set; }
        public bool HasAdminAccess { get; set; }

        // For relationships
        public int? FarmerId { get; set; }
        public int? GreenTechProviderId { get; set; }
    }
}
