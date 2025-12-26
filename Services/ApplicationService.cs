using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoleBasedApp.Data;

namespace RoleBasedApp.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _context.Roles
                .ToListAsync();
        }

        public async Task<List<IdentityUser>> GetAllUsersAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<IdentityUser> GetUserAsync(string id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<string>> GetUserRoleAsync(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> CheckAdminRoleAsync(IdentityUser user)
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            return users.Contains(user);
        }

        public async Task DeleteUser(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);

            if (user == null) return;

            var result = await _userManager.DeleteAsync(user);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    Console.WriteLine(error);
                }
            }
        }

        public async Task UpdateAdminRole(string userID, bool add)
        {
            var user = await _userManager.FindByIdAsync(userID);

            if (user == null) return;

            if (add)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
        }
    }
}
