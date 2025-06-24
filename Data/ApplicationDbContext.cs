using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    // ApplicationDbContext inherits from IdentityDbContext to integrate with ASP.NET Core Identity
    // This context class is responsible for accessing the database for user management and other entities.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor that accepts DbContextOptions and passes them to the base class constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSet properties for entities
        // These are used to query and save instances of the respective models
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }

        // Override OnModelCreating to configure the model and seed sample data (if needed)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure the base class model configuration is applied

            /* Uncomment this section to seed sample data for the Farmer table
            modelBuilder.Entity<Farmer>().HasData(
                new Farmer
                {
                    FarmerID = 1,
                    Name = "John Doe",
                    Contact = "1234567890",
                    Location = "Eastern Cape",
                    UserId = "dummy-id" // UserId should be replaced with a real User ID later
                }
            );
            */
        }
    }
}
