namespace RoleBasedApp.Models
{
    public static class Permissions
    {
        public const string ViewOrders = "CanViewOrders";
        public const string CreateOrders = "CanCreateOrders";
        public const string DeleteOrders = "CanDeleteOrders";

        public const string ViewProducts = "CanViewProducts";
        public const string CreateProducts = "CanCreateProducts";
        public const string DeleteProducts = "CanDeleteProducts";

        public const string ViewClients = "CanViewClients";
        public const string CreateClients = "CanCreateClients";
        public const string DeleteClients = "CanDeleteClients";

        public const string ViewUsers = "CanViewUsers";
        public const string CreateUsers = "CanCreateUsers";
        public const string DeleteUsers = "CanDeleteUsers";
    }
}
