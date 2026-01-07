using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoleBasedApp.Context;
using RoleBasedApp.Data;
using RoleBasedApp.Models;

namespace RoleBasedApp.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ITenantContext _tenant;

        public ApplicationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ITenantContext tenant)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tenant = tenant;
        }

        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Where(x => x.TenantId == _tenant.TenantId)
                .ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(x => x.TenantId == _tenant.TenantId)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            return await _context.Users
                .Where(x => x.TenantId == _tenant.TenantId)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IList<string>> GetUserRoleAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> CheckRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task DeleteUserAsync(string userID)
        {
            var user = await _context.Users
                .Where(u => u.TenantId == _tenant.TenantId)
                .FirstOrDefaultAsync(u => u.Id == userID);

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

        public async Task UpdateRoleAsync(string userID, string roleName, bool add)
        {
            var user = await _context.Users
                .Where(u => u.TenantId == _tenant.TenantId)
                .FirstOrDefaultAsync(u => u.Id == userID);

            if (user == null) return;

            if (add)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
        }

        public async Task AddRoleAsync(string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) == null)
            {
                var role = new ApplicationRole
                {
                    Name = roleName,
                    TenantId = _tenant.TenantId
                };
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
