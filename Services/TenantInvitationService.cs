using Microsoft.AspNetCore.Routing;
using RoleBasedApp.Data;
using RoleBasedApp.Models;

namespace RoleBasedApp.Services
{
    public class TenantInvitationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _email;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantInvitationService(ApplicationDbContext context, IEmailSender email, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _email = email;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InviteAsync(Guid tenantId, string email, string role)
        {
            var token = Guid.NewGuid().ToString();

            var invitation = new UserInvitation
            {
                Email = email,
                TenantId = tenantId,
                RoleName = role,
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7)
            };

            _context.UserInvitations.Add(invitation);
            await _context.SaveChangesAsync();

            var url = _linkGenerator.GetUriByPage(
                _httpContextAccessor.HttpContext!,
                page: "/InviteConfirmation",
                values: new { token }
            );

            await _email.SendAsync(
                email,
                "Invitation à rejoindre l’application",
                $"""
            <p>Vous avez été invité à rejoindre une organisation.</p>
            <p><a href="{url}">Cliquez ici pour accepter l'invitation</a></p>
            <p>Ce lien expire dans 7 jours.</p>
            """
            );
        }
    }

}
