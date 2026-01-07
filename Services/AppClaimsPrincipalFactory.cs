using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RoleBasedApp.Models;
using System.Security.Claims;

namespace RoleBasedApp.Services
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public AppClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> options)
        : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("TenantId", user.TenantId.ToString()));

            return identity;
        }
    }
}
