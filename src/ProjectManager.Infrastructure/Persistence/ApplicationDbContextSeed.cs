using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ProjectManager.Application.Constants;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            await EnsureRoleExists(roleManager, Roles.Administrator);

            await EnsureRoleExists(roleManager, Roles.BasicUser);

            var defaultAdmin = new ApplicationUser
            {
                UserName = "adminuser",
                Email = "admin@projectmanager.com",
                EmailConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultAdmin.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "SecurePass123!"); // Cambiar si va a producción
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Administrator);
                }
            }
        }

        private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

}
