using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.Data;

public abstract class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
        await context.Database.EnsureCreatedAsync();

        // Categories
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Id = 1, Name = "Software" },
                new() { Id = 2, Name = "Computer Parts" },
                new() { Id = 3, Name = "Peripherals" },
                new() { Id = 4, Name = "Accessories" },
                new() { Id = 5, Name = "Others" },
            };

            await context.AddRangeAsync(categories);
            
            // Subcategories
            if (!await context.Subcategories.AnyAsync())
            {
                var subCategories = new List<Subcategory>
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

                await context.AddRangeAsync(subCategories);
            }
        }
        
        // Brands
        if (!await context.Brands.AnyAsync())
        {
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

            await context.AddRangeAsync(brands);
        }

        await context.SaveChangesAsync();
    }
}