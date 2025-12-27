using Microsoft.AspNetCore.Identity;

namespace RoleBasedApp.Services
{
    public interface IApplicationService
    {
        Task<List<IdentityUser>> GetAllUsersAsync();
        Task<List<IdentityRole>> GetAllRolesAsync();

        Task<IdentityUser> GetUserAsync(string id);
        Task<IList<string>> GetUserRoleAsync(IdentityUser user);
        Task<bool> CheckRoleAsync(IdentityUser user, string roleName);

        Task DeleteUserAsync(string userID);
        Task UpdateRoleAsync(string userID, string roleName, bool add);
        Task AddRoleAsync(string roleName);
    }
}
