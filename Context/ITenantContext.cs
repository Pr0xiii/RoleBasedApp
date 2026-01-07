namespace RoleBasedApp.Context
{
    public interface ITenantContext
    {
        Guid TenantId { get; }
    }
}
