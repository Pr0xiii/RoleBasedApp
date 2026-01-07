using Microsoft.AspNetCore.Identity;

namespace RoleBasedApp.Models
{
    public class ApplicationRole : IdentityRole
    {
        public Guid TenantId { get; set; }
    }
}
