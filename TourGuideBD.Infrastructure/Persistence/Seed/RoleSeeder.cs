using Microsoft.AspNetCore.Identity;

namespace TourGuideBD.Infrastructure.Persistence.Seed;

public static class RoleSeeder
{
    public static readonly string[] Roles = { "User", "Moderator", "Admin", "TourGuide" };

    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<TourGuideBD.Domain.Entities.Users.ApplicationUser> userManager)
    {
        const string adminEmail = "admin@tourguidebd.com";
        const string adminPassword = "Admin@123";

        var existing = await userManager.FindByEmailAsync(adminEmail);
        if (existing == null)
        {
            var admin = new TourGuideBD.Domain.Entities.Users.ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Admin",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}