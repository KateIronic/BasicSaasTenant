using BasicSaasTenent.Models;

namespace BasicSaasTenent.Services
{
    public class TenancyManager : ITenancyManager
    {
        public Tenant? GetTenant(string tenantName) => tenantName switch
        {
            "in" => new Tenant() { Id = "in", Name = "India", IsActive = false },
            "us" => new Tenant() { Id = "us", Name = "USA", IsActive = false },
            "jp" => new Tenant() { Id = "jp", Name = "Japan", IsActive = false },
            "rs" => new Tenant() { Id = "rs", Name = "Russia", IsActive = false },
            "uk" => new Tenant() { Id = "uk", Name = "UK", IsActive = false },
            _ =>null
        };
    }
}
