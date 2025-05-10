using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add MVC services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Farmer", policy => policy.RequireRole("Farmer"));
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});

var app = builder.Build();

// Seed roles and users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    string[] roles = { "Farmer", "Employee" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed Farmer user
    var farmerEmail = "farmer@test.com";
    var farmerUser = await userManager.FindByEmailAsync(farmerEmail);
    if (farmerUser == null)
    {
        farmerUser = new IdentityUser
        {
            UserName = farmerEmail,
            Email = farmerEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(farmerUser, "Farmer1!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(farmerUser, "Farmer");

            // Create corresponding Farmer entity
            var farmer = new Farmer
            {
                Name = "John Doe",
                Contact = "1234567890",
                Location = "Eastern Cape",
                UserId = farmerUser.Id
            };
            dbContext.Farmers.Add(farmer);
            await dbContext.SaveChangesAsync();
        }
    }

    // Seed Employee user
    var employeeEmail = "employee@test.com";
    var employeeUser = await userManager.FindByEmailAsync(employeeEmail);
    if (employeeUser == null)
    {
        employeeUser = new IdentityUser
        {
            UserName = employeeEmail,
            Email = employeeEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(employeeUser, "Employee123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(employeeUser, "Employee");
        }
    }
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
