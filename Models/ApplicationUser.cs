using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
using System.Collections.Generic;

namespace AgriEnergyConnect.Models
{
    // Enum to define the user roles
    // This allows for easy categorization of user types such as Farmer and Employee
    public enum UserTypeEnum
    {
        Farmer,  // Represents a Farmer user type
        Employee // Represents an Employee user type
    }

    // ApplicationUser class extends IdentityUser to integrate with ASP.NET Core Identity
    // This class represents a user in the system with custom properties
    public class ApplicationUser : IdentityUser
    {
        // Enum to track the user type (Farmer or Employee)
        public UserTypeEnum UserType { get; set; } // Changed to enum for better control

        // Personal information
        public string FirstName { get; set; } // User's first name
        public string LastName { get; set; }  // User's last name

        // Access flags to define user capabilities
        public bool HasMarketplaceAccess { get; set; }  // Flag indicating if the user has marketplace access
        public bool HasForumAccess { get; set; }        // Flag indicating if the user has forum access
        public bool HasProductListingAccess { get; set; } // Flag indicating if the user can manage product listings
        public bool HasAdminAccess { get; set; }       // Flag indicating if the user has admin access

        // For relationships with other entities (e.g., Farmer or GreenTechProvider)
        public int? FarmerId { get; set; }              // Reference to Farmer's entity (nullable)
        public int? GreenTechProviderId { get; set; }   // Reference to GreenTechProvider's entity (nullable)
=======
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
>>>>>>> agri-part3/main
    }
}
