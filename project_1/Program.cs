using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using project_1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Создаем экземпляр RoleManager
var roleManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();

// Создаем роли, если они не существуют
if (!await roleManager.RoleExistsAsync("admin"))
{
    var role = new IdentityRole("admin");
    await roleManager.CreateAsync(role);
}
if (!await roleManager.RoleExistsAsync("viewer"))
{
    var role = new IdentityRole("viewer");
    await roleManager.CreateAsync(role);
}


// Создаем экземпляр UserManager
var userManager = app.Services.GetRequiredService<UserManager<IdentityUser>>();

// Создаем пользователя admin если он не существует, и присваиваем ему роль admin
if (await userManager.FindByNameAsync("admin") == null)
{
    var user = new IdentityUser { UserName = "admin" };
    var result = await userManager.CreateAsync(user, "Admin@123"); // пароль для пользователя admin

    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(user, "admin");
    }
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

