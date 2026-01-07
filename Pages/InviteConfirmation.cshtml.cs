using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoleBasedApp.Data;
using RoleBasedApp.Models;

namespace RoleBasedApp.Pages
{
    public class InviteConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public InviteConfirmationModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public UserInvitation Invitation { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmationPassword { get; set; }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            Invitation = await _context.UserInvitations
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    !x.Used &&
                    x.Expiration > DateTime.UtcNow);

            if (Invitation == null)
                return BadRequest("Invitation invalide");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string token)
        {
            var invitation = await _context.UserInvitations
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    !x.Used &&
                    x.Expiration > DateTime.UtcNow);

            if (invitation == null)
                return BadRequest();

            if (!ModelState.IsValid || Password != ConfirmationPassword)
                return Page();

            var user = new ApplicationUser
            {
                UserName = invitation.Email,
                Email = invitation.Email,
                TenantId = invitation.TenantId
            };

            var result = await _userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
                return Page();

            if (!string.IsNullOrEmpty(invitation.RoleName))
                await _userManager.AddToRoleAsync(user, invitation.RoleName);

            invitation.Used = true;
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, false);
            return RedirectToPage("/Index");
        }
    }
}
