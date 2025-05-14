using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
// The application uses SQLite as the database. The connection string "DefaultConnection" 
// is read from the app's configuration (e.g., appsettings.json).
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity with custom ApplicationUser
// Add ASP.NET Core Identity for handling user authentication, including password hashing, roles, etc.
// The Identity system is linked to the ApplicationDbContext, which will manage the user data.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>() // Store users and roles in the ApplicationDbContext
    .AddDefaultTokenProviders(); // Adds support for token generation, e.g., for password reset, email confirmation

// Register the dummy email sender
// A dummy email sender is used, probably for development purposes or testing.
// This will handle the email-sending logic (e.g., confirmation emails) in a simplified way.
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

// Add MVC services
// Add support for MVC controllers, views, and Razor Pages.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Authorization policies
// Define authorization policies for roles. This allows access control based on user roles.
builder.Services.AddAuthorization(options =>
{
    // Policy for Farmer role
    options.AddPolicy("Farmer", policy => policy.RequireRole("Farmer"));
    // Policy for Employee role
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});

var app = builder.Build();

// Seed roles and users
// This section initializes the database with roles (Farmer and Employee) and users (a farmer and an employee).
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); // Role manager for managing roles
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>(); // User manager for managing users
    var dbContext = services.GetRequiredService<ApplicationDbContext>(); // The database context to access the database

    string[] roles = { "Farmer", "Employee" }; // Define roles to be created if they don't exist

    // Create roles if they do not exist
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role)) // Check if the role already exists
        {
            await roleManager.CreateAsync(new IdentityRole(role)); // Create the role if not found
        }
    }

    // Seed Farmer user
    var farmerEmail = "farmer@test.com"; // Farmer's email for seeding
    var farmerUser = await userManager.FindByEmailAsync(farmerEmail); // Check if the farmer user exists
    if (farmerUser == null) // If the farmer user doesn't exist, create it
    {
        farmerUser = new ApplicationUser
        {
            UserName = farmerEmail, // Set username to email
            Email = farmerEmail, // Set email address
            EmailConfirmed = true, // Set the email to confirmed (for testing purposes)
            FirstName = "John", // Farmer's first name
            LastName = "Doe", // Farmer's last name
            PhoneNumber = "1234567890", // Farmer's phone number
            UserType = UserTypeEnum.Farmer // Set the user type to Farmer
        };

        // Create the farmer user with the password "Farmer1!" and add the "Farmer" role
        var result = await userManager.CreateAsync(farmerUser, "Farmer1!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(farmerUser, "Farmer"); // Add the Farmer role to the user

            // Create a corresponding Farmer entity and add it to the database
            var farmer = new Farmer
            {
                Name = "John Doe", // Farmer's name
                Contact = "1234567890", // Farmer's contact number
                Location = "Eastern Cape", // Farmer's location
                UserId = farmerUser.Id // Link the farmer with the ApplicationUser
            };
            dbContext.Farmers.Add(farmer); // Add farmer entity to the database
            await dbContext.SaveChangesAsync(); // Save changes to the database
        }
    }

    // Seed Employee user
    var employeeEmail = "employee@test.com"; // Employee's email for seeding
    var employeeUser = await userManager.FindByEmailAsync(employeeEmail); // Check if the employee user exists
    if (employeeUser == null) // If the employee user doesn't exist, create it
    {
        employeeUser = new ApplicationUser
        {
            UserName = employeeEmail, // Set username to email
            Email = employeeEmail, // Set email address
            EmailConfirmed = true, // Set the email to confirmed
            FirstName = "Emily", // Employee's first name
            LastName = "Smith", // Employee's last name
            PhoneNumber = "0987654321", // Employee's phone number
            UserType = UserTypeEnum.Employee // Set the user type to Employee
        };

        // Create the employee user with the password "Employee123!" and add the "Employee" role
        var result = await userManager.CreateAsync(employeeUser, "Employee123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(employeeUser, "Employee"); // Add the Employee role to the user
        }
    }
}

// Middleware pipeline
// Set up middleware components for error handling, HTTPS redirection, static files, authentication, and authorization.
if (app.Environment.IsDevelopment()) // If the environment is development, show detailed error pages
{
    app.UseDeveloperExceptionPage();
}
else // In production, use exception handling and HTTP Strict Transport Security (HSTS)
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles(); // Serve static files (CSS, JS, images, etc.)

app.UseRouting(); // Enable routing for controllers and views

app.UseAuthentication(); // Enable authentication middleware to handle user sign-ins
app.UseAuthorization(); // Enable authorization middleware to manage access control based on roles and policies

// Define the default route for MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Map Razor Pages

app.Run(); // Run the application
