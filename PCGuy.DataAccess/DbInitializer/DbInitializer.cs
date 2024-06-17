using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCGuy.DataAccess.Data;
using PCGuy.Helpers;
using PCGuy.Models.Entities;

namespace PCGuy.DataAccess.DbInitializer;

public class DbInitializer(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ApplicationDbContext db) : IDbInitializer
{
    public async Task InitializeAsync()
    {
        // Step 1: Migrations if not applied
        try
        {
            if ((await db.Database.GetPendingMigrationsAsync()).Any())
            {
                await db.Database.MigrateAsync();
            }
        }
        catch
        {
            throw new ArgumentNullException();
        }

        // Step 2: Create Roles
        if (!await roleManager.RoleExistsAsync(Roles.CUSTOMER))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.CUSTOMER));
            await roleManager.CreateAsync(new IdentityRole(Roles.COMPANY));
            await roleManager.CreateAsync(new IdentityRole(Roles.ADMIN));
            await roleManager.CreateAsync(new IdentityRole(Roles.EMPLOYEE));
            
            // Step 3: Add Admin user
            await userManager.CreateAsync(
                new ApplicationUser
                {
                    UserName = "hi@reddavid.me",
                    Email = "hi@reddavid.me",
                    Name = "David Ballesteros",
                    PhoneNumber = "111222333444",
                    StreetAddress = "99 Corner Circle",
                    State = "NCR",
                    PostalCode = "1220",
                    City = "QC"
                }, "Admin123!@#");

            ApplicationUser? user = await db.ApplicationUsers.FirstOrDefaultAsync(o => o.Email == "hi@reddavid.me");
        
            if (user is not null)
            {
                await userManager.AddToRoleAsync(user, Roles.ADMIN);
            }
        }
        
    }
}