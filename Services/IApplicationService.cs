using RoleBasedApp.Models;

namespace RoleBasedApp.Services
{
    public interface IApplicationService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<List<ApplicationRole>> GetAllRolesAsync();

        Task<ApplicationUser> GetUserAsync(string id);
        Task<IList<string>> GetUserRoleAsync(ApplicationUser user);
        Task<bool> CheckRoleAsync(ApplicationUser user, string roleName);

        Task DeleteUserAsync(string userID);
        Task UpdateRoleAsync(string userID, string roleName, bool add);
        Task AddRoleAsync(string roleName);
    }
}
