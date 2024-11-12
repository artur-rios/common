using Newtonsoft.Json;
using TechCraftsmen.Core.WebApi.Security.Attributes;
using TechCraftsmen.Core.WebApi.Security.Interfaces;

namespace TechCraftsmen.Core.WebApi.Security.Middleware;

public class JwtMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IAuthenticationService authService)
    {
        var endpoint = context.GetEndpoint();
            
        if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() is null)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? "";
            var validationOutput = authService.ValidateTokenAndGetUser(token);

            if (validationOutput.Success)
            {
                context.Items["User"] = validationOutput.Data;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                
                var payload = JsonConvert.SerializeObject(validationOutput);

                await context.Response.WriteAsync(payload);

                return;
            }
        }
            
        await next(context);
    }
}
