using Microsoft.AspNetCore.Identity;

namespace RoleBasedApp.Services
{
    public interface IApplicationService
    {
        Task<List<IdentityUser>> GetAllUsersAsync();
        Task<List<IdentityRole>> GetAllRolesAsync();

        Task<IdentityUser> GetUserAsync(string id);
        Task<IList<string>> GetUserRoleAsync(IdentityUser user);
        Task<bool> CheckAdminRoleAsync(IdentityUser user);

        Task DeleteUser(string userID);
        Task UpdateAdminRole(string userID, bool add);
    }
}
