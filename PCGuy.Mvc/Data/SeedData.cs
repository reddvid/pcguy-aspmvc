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
        // var categories = new List<Category>
        // {
        //     new() { Id = 1, Name = "Software" },
        //     new() { Id = 2, Name = "Computer Parts" },
        //     new() { Id = 3, Name = "Peripherals" },
        //     new() { Id = 4, Name = "Accessories" },
        //     new() { Id = 5, Name = "Others" },
        // };
        //
        // await context.AddRangeAsync(categories);
        //
        // var subCategories = new List<Subcategory>
        // {
        //     new() { Id = 1, Name = "Audio", CategoryId = categories[2].Id },
        //     new() { Id = 2, Name = "Monitors", CategoryId = categories[2].Id },
        //     new() { Id = 3, Name = "Mice", CategoryId = categories[2].Id },
        //     new() { Id = 4, Name = "Keyboard", CategoryId = categories[2].Id },
        //     new() { Id = 5, Name = "Cables", CategoryId = categories[3].Id },
        //     new() { Id = 6, Name = "Controllers", CategoryId = categories[2].Id },
        //     new() { Id = 7, Name = "Headphones", CategoryId = categories[2].Id },
        //     new() { Id = 8, Name = "Headset", CategoryId = categories[2].Id },
        //     new() { Id = 9, Name = "Microphones", CategoryId = categories[2].Id },
        //     new() { Id = 10, Name = "Wireless Routers", CategoryId = categories[3].Id },
        //     new() { Id = 11, Name = "Memory", CategoryId = categories[1].Id },
        //     new() { Id = 12, Name = "Storage", CategoryId = categories[1].Id },
        //     new() { Id = 13, Name = "Webcam", CategoryId = categories[2].Id },
        //     new() { Id = 14, Name = "HDD", CategoryId = categories[1].Id },
        //     new() { Id = 15, Name = "SSD", CategoryId = categories[1].Id },
        //     new() { Id = 16, Name = "CPU", CategoryId = categories[1].Id },
        //     new() { Id = 17, Name = "GPU", CategoryId = categories[1].Id },
        //     new() { Id = 18, Name = "Motherboard", CategoryId = categories[1].Id },
        //     new() { Id = 19, Name = "CPU Fans", CategoryId = categories[1].Id },
        //     new() { Id = 20, Name = "Case Fans", CategoryId = categories[3].Id },
        //     new() { Id = 21, Name = "Miscellaneous", CategoryId = categories[3].Id },
        // };
        //
        // await context.AddRangeAsync(subCategories);
        //
        // var brands = new List<Brand>
        // {
        //     new() { Id = 1, Name = "Unbranded" },
        //     new() { Id = 2, Name = "AMD" },
        //     new() { Id = 3, Name = "NVIDIA" },
        //     new() { Id = 4, Name = "Audio-Technica" },
        //     new() { Id = 5, Name = "Logitech" },
        //     new() { Id = 6, Name = "Keychron" },
        //     new() { Id = 7, Name = "Xitrix" },
        //     new() { Id = 8, Name = "Samsung" },
        //     new() { Id = 9, Name = "XBOX" },
        //     new() { Id = 10, Name = "HyperX" },
        //     new() { Id = 11, Name = "Elgato" },
        //     new() { Id = 12, Name = "Samson" },
        //     new() { Id = 13, Name = "Razer" },
        //     new() { Id = 14, Name = "Gigabyte" },
        //     new() { Id = 15, Name = "ZOTAC" },
        //     new() { Id = 16, Name = "PreSonus" },
        //     new() { Id = 17, Name = "MSI" },
        //     new() { Id = 18, Name = "Kingston" },
        //     new() { Id = 19, Name = "PNY" },
        //     new() { Id = 20, Name = "Seagate" },
        //     new() { Id = 21, Name = "Intel" },
        //     new() { Id = 22, Name = "Corsair" },
        //     new() { Id = 23, Name = "SilverStone" },
        // };
        //
        // await context.AddRangeAsync(brands);
        //
        // var products = new List<Product>
        // {
        //     new()
        //     {
        //         Name = "AMD Ryzen 5 5700X", UploadDate = DateTime.Now, Price = 11_000,
        //         BrandId = brands[0].Id,
        //         SubcategoryId = subCategories[15].Id
        //     },
        //     new()
        //     {
        //         Name = "MSI B550 PRO-VDH WIFI", UploadDate = DateTime.Now, Price = 2_500,
        //         BrandId = brands[15].Id,
        //         SubcategoryId = subCategories[16].Id
        //     },
        //     new()
        //     {
        //         Name = "SilverStone PF240-ARGB", UploadDate = DateTime.Now, Price = 1_500,
        //         BrandId = brands[21].Id,
        //         SubcategoryId = subCategories[17].Id
        //     },
        //     new()
        //     {
        //         Name = "HyperX Fury 16GB", UploadDate = DateTime.Now, Price = 2_500,
        //         BrandId = brands[16].Id,
        //         SubcategoryId = subCategories[10].Id
        //     },
        //     new()
        //     {
        //         Name = "Samsung 970 EVO Plus", UploadDate = DateTime.Now, Price = 4_500,
        //         BrandId = brands[6].Id,
        //         SubcategoryId = subCategories[14].Id
        //     },
        // };

        //await context.AddRangeAsync(products);

        await context.SaveChangesAsync();
    }
}