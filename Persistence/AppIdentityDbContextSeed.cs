using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppUserRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                List<AppUserRole> roles = new List<AppUserRole>
                {
                     new AppUserRole{Name="Admin", DescriptionAr="Admin", DescriptionEn="Admin", FullDescription="Admin"},
                     new AppUserRole{Name="Employee", DescriptionAr="Employee", DescriptionEn="Employee", FullDescription="Employee"},
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
