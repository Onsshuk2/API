using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.DAL
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            string[] roleNames = { "admin", "user" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }

            if (await userManager.Users.AllAsync(u => u.UserName != "admin"))
            {
                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");
                }
            }

            if (await userManager.Users.AllAsync(u => u.UserName != "user"))
            {
                var normalUser = new AppUser
                {
                    UserName = "user",
                    Email = "user@example.com",
                    FirstName = "Regular",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(normalUser, "User123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "user");
                }
            }
        }
    }
}
