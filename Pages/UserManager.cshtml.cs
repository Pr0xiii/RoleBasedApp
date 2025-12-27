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

        [BindProperty]
        public string Name { get; set; }

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

        public async Task<bool> CheckRoleAsync(IdentityUser user, string roleName)
        {
            return await _service.CheckRoleAsync(user, roleName);
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string userID)
        {
            await _service.DeleteUserAsync(userID);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateRoleAsync(string userID, string roleName, bool add)
        {
            await _service.UpdateRoleAsync(userID, roleName, add);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateRoleAsync()
        {
            if (!ModelState.IsValid || Name == null)
                return Page();

            await _service.AddRoleAsync(Name);

            return RedirectToPage();
        }
    }
}
