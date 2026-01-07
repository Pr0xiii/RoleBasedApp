using Microsoft.AspNetCore.Identity;

namespace RoleBasedApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid TenantId { get; set; }
    }
}
