
namespace RoleBasedApp.Context
{
    public class TenantContext : ITenantContext
    {
        public Guid TenantId { get; }

        public TenantContext(IHttpContextAccessor accessor)
        {
            var value = accessor.HttpContext?.User?
            .FindFirst("TenantId")?.Value;

            if (Guid.TryParse(value, out var tenantId))
                TenantId = tenantId;
        }
    }
}
