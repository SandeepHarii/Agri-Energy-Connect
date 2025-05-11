using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity with custom ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Register the dummy email sender
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

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
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
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
        farmerUser = new ApplicationUser
        {
            UserName = farmerEmail,
            Email = farmerEmail,
            EmailConfirmed = true,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            UserType = UserTypeEnum.Farmer
        };

        var result = await userManager.CreateAsync(farmerUser, "Farmer1!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(farmerUser, "Farmer");

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
        employeeUser = new ApplicationUser
        {
            UserName = employeeEmail,
            Email = employeeEmail,
            EmailConfirmed = true,
            FirstName = "Emily",
            LastName = "Smith",
            PhoneNumber = "0987654321",
            UserType = UserTypeEnum.Employee
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
