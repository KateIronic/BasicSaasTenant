using BasicSaasTenent.Models;
using BasicSaasTenent.Services;

public class TenancyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenancyMiddleware> _logger;

    public TenancyMiddleware(RequestDelegate next, ILogger<TenancyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenancyManager tenancyManager,
        ITenantSetter tenantSetter,
        ITenantGetter tenantGetter)
    {
        var tenantCode = context.Request.Headers["X-TenantCode"].FirstOrDefault();

        if (tenantCode is null)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                status = "TENANT_IS_MISSING",
                message = "Request path is missing."
            });
            return;
        } 
        var currentTenant = tenancyManager.GetTenant(tenantCode);

        if (currentTenant is null)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                status = "TENANT_IS_NOT_REGISTERED",
                message = $"Tenant {tenantCode} is not registered."
            });
            return;
        }
        
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userTenantId = context.User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;

            if (userTenantId != currentTenant.Id)
            {
                _logger.LogWarning("User does not belong to tenant {TenantName}.", tenantCode);
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new
                {
                    status = "TENANT_ACCESS_DENIED",
                    message = "User does not belong to the identified tenant."
                });
                return;
            }
        }


        // Set tenant properties
        tenantSetter.Id = currentTenant.Id;
        tenantSetter.Name = currentTenant.Name;
        tenantSetter.IsActive = true;
        context.Items["TenantId"] = currentTenant.Id;

        context.Items["TenantId"] = tenantGetter.Id;

        await _next(context);
    }
}
