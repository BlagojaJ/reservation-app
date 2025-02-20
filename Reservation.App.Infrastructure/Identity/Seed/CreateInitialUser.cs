using Microsoft.AspNetCore.Identity;
using Reservation.App.Infrastructure.Identity.Models;

namespace Reservation.App.Infrastructure.Identity.Seed
{
    public static class UserCreator
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var applicationUser = new ApplicationUser
            {
                FirstName = "User",
                LastName = "Userski",
                UserName = "user-userski",
                Email = "user@company.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(applicationUser, "Password1!");
            }
        }
    }
}
