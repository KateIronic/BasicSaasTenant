using BasicSaasTenent.Models;

namespace BasicSaasTenent.Services
{
    public interface ITenancyManager
    {
        Tenant? GetTenant(string tenantName);
    }
}
