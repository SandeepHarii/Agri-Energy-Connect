using Microsoft.AspNetCore.Identity;
using System;

namespace AgriEnergyConnect.Models
{
    public enum UserTypeEnum
    {
        Farmer,
        Employee
    }

    public enum FarmerStatus
    {
        Pending,
        Active
    }

    public class ApplicationUser : IdentityUser
    {
        public UserTypeEnum UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool HasMarketplaceAccess { get; set; }
        public bool HasForumAccess { get; set; }
        public bool HasProductListingAccess { get; set; }
        public bool HasAdminAccess { get; set; }
        public int? FarmerId { get; set; }
        public int? GreenTechProviderId { get; set; }
        public string? RegisteredById { get; set; }
        public ApplicationUser RegisteredBy { get; set; }
        public FarmerStatus Status { get; set; } = FarmerStatus.Pending;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
