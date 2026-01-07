namespace RoleBasedApp.Models
{
    public class UserInvitation
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public Guid TenantId { get; set; }

        public string RoleName { get; set; }
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
        public bool Used { get; set; }
    }
}
