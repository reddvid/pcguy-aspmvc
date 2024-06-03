using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Category> Categories { get; init; }
    public DbSet<Subcategory> Subcategories { get; init; }
    public DbSet<Brand> Brands { get; init; }
    public DbSet<Product> Products { get; init; }

    public DbSet<ApplicationUser> ApplicationUsers { get; init; }
    public DbSet<ShoppingCart> ShoppingCarts { get; init; }
    public DbSet<Company> Companies { get; init; }
    public DbSet<OrderHeader> OrderHeaders { get; init; }
    public DbSet<OrderDetail> OrderDetails { get; init; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Software" },
            new() { Id = 2, Name = "Computer Parts" },
            new() { Id = 3, Name = "Peripherals" },
            new() { Id = 4, Name = "Accessories" },
            new() { Id = 5, Name = "Others" },
        };

        var subcategories = new List<Subcategory>
        {
            new() { Id = 1, Name = "Audio", CategoryId = categories[2].Id },
            new() { Id = 2, Name = "Monitors", CategoryId = categories[2].Id },
            new() { Id = 3, Name = "Mice", CategoryId = categories[2].Id },
            new() { Id = 4, Name = "Keyboard", CategoryId = categories[2].Id },
            new() { Id = 5, Name = "Cables", CategoryId = categories[3].Id },
            new() { Id = 6, Name = "Controllers", CategoryId = categories[2].Id },
            new() { Id = 7, Name = "Headphones", CategoryId = categories[2].Id },
            new() { Id = 8, Name = "Headset", CategoryId = categories[2].Id },
            new() { Id = 9, Name = "Microphones", CategoryId = categories[2].Id },
            new() { Id = 10, Name = "Wireless Routers", CategoryId = categories[3].Id },
            new() { Id = 11, Name = "Memory", CategoryId = categories[1].Id },
            new() { Id = 12, Name = "Storage", CategoryId = categories[1].Id },
            new() { Id = 13, Name = "Webcam", CategoryId = categories[2].Id },
            new() { Id = 14, Name = "HDD", CategoryId = categories[1].Id },
            new() { Id = 15, Name = "SSD", CategoryId = categories[1].Id },
            new() { Id = 16, Name = "CPU", CategoryId = categories[1].Id },
            new() { Id = 17, Name = "GPU", CategoryId = categories[1].Id },
            new() { Id = 18, Name = "Motherboard", CategoryId = categories[1].Id },
            new() { Id = 19, Name = "CPU Fans", CategoryId = categories[1].Id },
            new() { Id = 20, Name = "Case Fans", CategoryId = categories[3].Id },
            new() { Id = 21, Name = "Miscellaneous", CategoryId = categories[3].Id },
        };

        var brands = new List<Brand>
        {
            new() { Id = 1, Name = "Unbranded" },
            new() { Id = 2, Name = "AMD" },
            new() { Id = 3, Name = "NVIDIA" },
            new() { Id = 4, Name = "Audio-Technica" },
            new() { Id = 5, Name = "Logitech" },
            new() { Id = 6, Name = "Keychron" },
            new() { Id = 7, Name = "Xitrix" },
            new() { Id = 8, Name = "Samsung" },
            new() { Id = 9, Name = "XBOX" },
            new() { Id = 10, Name = "HyperX" },
            new() { Id = 11, Name = "Elgato" },
            new() { Id = 12, Name = "Samson" },
            new() { Id = 13, Name = "Razer" },
            new() { Id = 14, Name = "Gigabyte" },
            new() { Id = 15, Name = "ZOTAC" },
            new() { Id = 16, Name = "PreSonus" },
            new() { Id = 17, Name = "MSI" },
            new() { Id = 18, Name = "Kingston" },
            new() { Id = 19, Name = "PNY" },
            new() { Id = 20, Name = "Seagate" },
            new() { Id = 21, Name = "Intel" },
            new() { Id = 22, Name = "Corsair" },
            new() { Id = 23, Name = "SilverStone" },
        };

        var products = new List<Product>
        {
            new()
            {
                Id = 1,
                Name = "AMD Ryzen 5 5700X",
                Price = 11_000,
                BrandId = brands[0].Id,
                SubcategoryId = subcategories[15].Id
            },
            new()
            {
                Id = 2,
                Name = "MSI B550 PRO-VDH WIFI",
                Price = 2_500,
                BrandId = brands[15].Id,
                SubcategoryId = subcategories[16].Id
            },
            new()
            {
                Id = 3,
                Name = "SilverStone PF240-ARGB",
                Price = 1_500,
                BrandId = brands[21].Id,
                SubcategoryId = subcategories[17].Id
            },
            new()
            {
                Id = 4,
                Name = "HyperX Fury 16GB",
                Price = 2_500,
                BrandId = brands[16].Id,
                SubcategoryId = subcategories[10].Id
            },
            new()
            {
                Id = 5,
                Name = "Samsung 970 EVO Plus",
                Price = 4_500,
                BrandId = brands[6].Id,
                SubcategoryId = subcategories[14].Id
            },
        };

        var companies = new List<Company>
        {
            new()
            {
                Id = 1, Name = "NuTech", StreetAddress = "123 Tech St", City = "Tokyo", PostalCode = "57000",
                State = "NY", PhoneNumber = "000012345"
            },
            new()
            {
                Id = 2, Name = "InfoSystems Grid", StreetAddress = "45 Grid St", City = "Makati", PostalCode = "5100",
                State = "PH", PhoneNumber = "990012345"
            },
            new()
            {
                Id = 3, Name = "Krytech Solutions", StreetAddress = "10 Solutions St", City = "Baguio",
                PostalCode = "4100",
                State = "PH", PhoneNumber = "630012345"
            },
        };

        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Subcategory>().HasData(subcategories);
        modelBuilder.Entity<Brand>().HasData(brands);
        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<Company>().HasData(companies);
    }
}