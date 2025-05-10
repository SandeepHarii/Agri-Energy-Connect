using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           /* // Sample data
            modelBuilder.Entity<Farmer>().HasData(
                new Farmer
                {
                    FarmerID = 1,
                    Name = "John Doe",
                    Contact = "1234567890",
                    Location = "Eastern Cape",
                    UserId = "dummy-id" // Will be replaced later
                }
            );*/
        }
    }

}
