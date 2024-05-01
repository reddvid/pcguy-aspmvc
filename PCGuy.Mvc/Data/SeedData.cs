using Microsoft.EntityFrameworkCore;
using PCGuy.Common.Entities;

namespace PCGuy.Mvc.Data;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        await using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
        await context.Database.EnsureCreatedAsync();

        if (await context.Products.AnyAsync()) return;

        // Categories
        var categories = new List<Category>
        {
            new() { Name = "Software" },
            new() { Name = "Computer Parts" },
            new() { Name = "Peripherals" },
            new() { Name = "Accessories" }
        };

        await context.AddRangeAsync(categories);

        var subCategories = new List<Subcategory>
        {
            new() { Name = "Audio", Category = categories[2] },
            new() { Name = "Monitors", Category = categories[2] },
            new() { Name = "Mice", Category = categories[2] },
            new() { Name = "Keyboard", Category = categories[2] },
            new() { Name = "Cables", Category = categories[3] },
            new() { Name = "Controllers", Category = categories[2] },
            new() { Name = "Headphones", Category = categories[2] },
            new() { Name = "Headset", Category = categories[2] },
            new() { Name = "Microphones", Category = categories[2] },
            new() { Name = "Wireless Routers", Category = categories[3] },
            new() { Name = "Memory", Category = categories[1] },
            new() { Name = "Storage", Category = categories[1] },
            new() { Name = "Webcam", Category = categories[2] },
            new() { Name = "HDD", Category = categories[1] },
            new() { Name = "SSD", Category = categories[1] },
            new() { Name = "CPU", Category = categories[1] },
            new() { Name = "Motherboard", Category = categories[1] },
            new() { Name = "CPU Fans", Category = categories[1] },
            new() { Name = "Case Fans", Category = categories[3] },
        };

        await context.AddRangeAsync(subCategories);

        var brands = new List<Brand>
        {
            new() { Name = "AMD" },
            new() { Name = "NVIDIA" },
            new() { Name = "Audio-Technica" },
            new() { Name = "Logitech" },
            new() { Name = "Keychron" },
            new() { Name = "Xitrix" },
            new() { Name = "Samsung" },
            new() { Name = "XBOX" },
            new() { Name = "HyperX" },
            new() { Name = "Elgato" },
            new() { Name = "Samson" },
            new() { Name = "Razer" },
            new() { Name = "Gigabyte" },
            new() { Name = "ZOTAC" },
            new() { Name = "PreSonus" },
            new() { Name = "MSI" },
            new() { Name = "Kingston" },
            new() { Name = "PNY" },
            new() { Name = "Seagate" },
            new() { Name = "Intel" },
            new() { Name = "Corsair" },
            new() { Name = "SilverStone" },
        };

        await context.AddRangeAsync(brands);

        var products = new List<Product>
        {
            new()
            {
                Name = "AMD Ryzen 5 5700X", UploadDate = DateTime.Now, Price = 11_000,
                Brand = brands[0],
                SubCategory = subCategories[15]
            },
            new()
            {
                Name = "MSI B550 PRO-VDH WIFI", UploadDate = DateTime.Now, Price = 2_500,
                Brand = brands[15],
                SubCategory = subCategories[16]
            },
            new()
            {
                Name = "SilverStone PF240-ARGB", UploadDate = DateTime.Now, Price = 1_500,
                Brand = brands[21],
                SubCategory = subCategories[17]
            },
            new()
            {
                Name = "HyperX Fury 16GB", UploadDate = DateTime.Now, Price = 2_500,
                Brand = brands[16],
                SubCategory = subCategories[10]
            },
            new()
            {
                Name = "Samsung 970 EVO Plus", UploadDate = DateTime.Now, Price = 4_500,
                Brand = brands[6],
                SubCategory = subCategories[14]
            },
        };

        await context.AddRangeAsync(products);

        await context.SaveChangesAsync();
    }
}