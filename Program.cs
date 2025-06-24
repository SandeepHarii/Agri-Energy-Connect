using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. DB Context (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity with custom user
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3. Email sender config
var emailConfig = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddSingleton<IEmailSender>(new SmtpEmailSender(
    emailConfig.SmtpServer,
    emailConfig.Port,
    emailConfig.FromEmail,
    emailConfig.Password
));

// 4. MVC + Razor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// 5. Role-based policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Farmer", policy => policy.RequireRole("Farmer"));
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});

var app = builder.Build();

// 6. Seed roles and default users
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
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Farmer
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

            dbContext.Farmers.Add(new Farmer
            {
                Name = "John Doe",
                Contact = "1234567890",
                Location = "Eastern Cape",
                UserId = farmerUser.Id
            });

            await dbContext.SaveChangesAsync();
        }
    }

    // Employee
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
            await userManager.AddToRoleAsync(employeeUser, "Employee");
    }
}

// 7. Middleware
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
