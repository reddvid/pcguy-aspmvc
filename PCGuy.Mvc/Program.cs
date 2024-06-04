using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PCGuy.DataAccess.Contracts;
using PCGuy.DataAccess.Data;
using PCGuy.DataAccess.Repository;
using PCGuy.Helpers;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    // await SeedData.Initialize(services);
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// app.MapControllerRoute(
//     name: "products",
//     pattern: "products/peripherals",
//     defaults: new { controller = "Product", action = "Index", id = 3 });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products/pc-parts",
//     defaults: new { controller = "Product", action = "Index", id = 2 });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products/software",
//     defaults: new { controller = "Product", action = "Index", id = 1 });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products/{id?}",
//     defaults: new { controller = "Product", action="Index" });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products",
//     defaults: new { controller = "Product", action="Index", id = 0 });
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{productId?}");
app.MapRazorPages();

app.Run();