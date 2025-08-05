// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: this middleware is meant to be used in other projects

using ArturRios.Common.WebApi.Security.Attributes;
using ArturRios.Common.WebApi.Security.Interfaces;
using Newtonsoft.Json;

namespace ArturRios.Common.WebApi.Security.Middleware;

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
