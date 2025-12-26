using Microsoft.AspNetCore.Identity;

namespace RoleBasedApp.Models
{
    public class IdentityConfig
    {
        public static async Task CreateAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var username = "admin@test.com";
            var password = "Admin123!";
            var roleName = "Admin";

            if(await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if(await userManager.FindByNameAsync(username) == null)
            {
                IdentityUser user = new IdentityUser { UserName = username, Email = username, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
