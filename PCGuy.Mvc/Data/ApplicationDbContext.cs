using Humanizer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCGuy.Common.Entities;

namespace PCGuy.Mvc.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Subcategory> SubCategories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        //
        // modelBuilder.Entity<Category>().HasData(categories);
        // modelBuilder.Entity<Subcategory>().HasData(subCategories);
        // modelBuilder.Entity<Brand>().HasData(brands);
        // modelBuilder.Entity<Product>().HasData(products);

        base.OnModelCreating(modelBuilder);
    }
}