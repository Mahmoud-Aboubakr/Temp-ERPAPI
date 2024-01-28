using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailFromClaimsPrinciple(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            var userEmail = user.FindFirstValue(ClaimTypes.Email);

            var appUser = await userManager.FindByEmailAsync(userEmail);

            if (appUser is null)
                throw new Exception($"{nameof(appUser.Email)}, {userEmail} not found");

            return appUser;
        }
    }
}
