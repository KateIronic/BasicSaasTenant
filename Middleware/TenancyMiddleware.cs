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
        var path = context.Request.Path;

        if (!path.HasValue)
        {
            _logger.LogWarning("Request path is missing.");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                status = "TENANT_IS_MISSING",
                message = "Request path is missing."
            });
            return;
        }

        // Split the path into segments
        var segments = path.Value
            .Split("/", StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        if (segments.Length < 1)
        {
            _logger.LogWarning("Tenant is missing in the path.");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                status = "TENANT_IS_MISSING",
                message = "Tenant is missing from the request."
            });
            return;
        }

        // Extract tenant name
        var tenantName = segments[0];
        var currentTenant = tenancyManager.GetTenant(tenantName);

        if (currentTenant == null)
        {
            _logger.LogWarning("Tenant {TenantName} is not registered.", tenantName);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new
            {
                status = "TENANT_NOT_FOUND",
                message = $"Tenant {tenantName} is not registered."
            });
            return;
        }
        //if (context.User.Identity?.IsAuthenticated != true)
        //{
        //    _logger.LogWarning("Unauthorized access attempt.");
        //    context.Response.StatusCode = 401; // Unauthorized
        //    await context.Response.WriteAsJsonAsync(new
        //    {
        //        status = "UNAUTHORIZED",
        //        message = "You must be authenticated to access this resource."
        //    });
        //    return;
        //}
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userTenantId = context.User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;

            if (userTenantId != currentTenant.Id)
            {
                _logger.LogWarning("User does not belong to tenant {TenantName}.", tenantName);
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

        // Pass tenant info through HttpContext.Items
        context.Items["TenantId"] = tenantGetter.Id;

        // Update request path correctly
        context.Request.PathBase = new PathString($"/{tenantName}");
        context.Request.Path = new PathString("/" + string.Join("/", segments.Skip(1)));

        _logger.LogInformation("Tenant {TenantName} identified and processed.", tenantName);

        // Continue processing the request
        await _next(context);
    }
}
