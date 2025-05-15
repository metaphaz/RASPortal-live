using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RASPortal.Models;
using System;
using System.Threading.Tasks;

namespace RASPortal.Data
{
    public static class SeedData
    {
        public static async Task CreateRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // Get RoleManager and UserManager from DI container
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Define roles to be added
            string[] roles = { "User", "Manager", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create a default admin user if it does not exist
            var adminEmail = "yigitmuhittinozan@ieee.org";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "metaphaz_admin",
                    Email = adminEmail,
                    FirstName = "Yiğit",
                    LastName = "Özan",
                    DateOfBirth = DateTime.Now.AddYears(-21)
                };

                var result = await userManager.CreateAsync(newAdmin, "//IEEE_IUC_2025_16m@//");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                else
                {
                    // Handle errors (log, throw, etc.)
                    throw new Exception("Failed to create default admin user.");
                }
            }
        }
    }
}