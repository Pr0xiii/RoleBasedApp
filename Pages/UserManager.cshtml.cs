using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoleBasedApp.Services;
using System.Threading.Tasks;

namespace RoleBasedApp.Pages
{
    [Authorize(Roles = "Admin")]
    public class UserManagerModel : PageModel
    {
        private readonly IApplicationService _service;
        
        public UserManagerModel(IApplicationService service)
        {
            _service = service;
        }

        public List<IdentityUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _service.GetAllUsersAsync();

            Roles = await _service.GetAllRolesAsync();

            return Page();
        }

        public async Task<IList<string>> GetUserRoleAsync(IdentityUser user)
        {
            return await _service.GetUserRoleAsync(user);
        }

        public async Task<bool> CheckAdminRoleAsync(IdentityUser user)
        {
            return await _service.CheckAdminRoleAsync(user);
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string userID)
        {
            await _service.DeleteUser(userID);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAdminRoleAsync(string userID, bool add)
        {
            await _service.UpdateAdminRole(userID, add);

            return RedirectToPage();
        }
    }
}
