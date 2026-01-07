using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoleBasedApp.Models;
using RoleBasedApp.Services;
using System.Threading.Tasks;

namespace RoleBasedApp.Pages
{
    [Authorize(Policy = Permissions.ViewUsers)]
    public class UserManagerModel : PageModel
    {
        private readonly IApplicationService _service;
        private readonly TenantInvitationService _tenantInvitationService;
        
        public UserManagerModel(IApplicationService service, TenantInvitationService invitationService)
        {
            _service = service;
            _tenantInvitationService = invitationService;
        }

        public List<ApplicationUser> Users { get; set; }
        public List<ApplicationRole> Roles { get; set; }

        //[BindProperty]
        //public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _service.GetAllUsersAsync();

            Roles = await _service.GetAllRolesAsync();

            return Page();
        }

        public async Task<IList<string>> GetUserRoleAsync(ApplicationUser user)
        {
            return await _service.GetUserRoleAsync(user);
        }

        public async Task<bool> CheckRoleAsync(ApplicationUser user, string roleName)
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

        //public async Task<IActionResult> OnPostCreateRoleAsync()
        //{
        //    if (!ModelState.IsValid || Name == null)
        //        return Page();

        //    await _service.AddRoleAsync(Name);

        //    return RedirectToPage();
        //}

        public async Task<IActionResult> OnPostInviteUserAsync()
        {
            if (!ModelState.IsValid || Email == null)
                return Page();

            var _tenantId = HttpContext.User.FindFirst("TenantId").Value;
            await _tenantInvitationService.InviteAsync(Guid.Parse(_tenantId), Email, "User");

            return RedirectToPage();
        }
    }
}
