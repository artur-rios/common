// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: this middleware is meant to be used in other projects

using ArturRios.Common.Configuration.Providers;
using ArturRios.Common.Output;
using ArturRios.Common.Security;
using ArturRios.Common.Web.Api.Configuration;
using ArturRios.Common.Web.Middleware;
using ArturRios.Common.Web.Security.Attributes;
using ArturRios.Common.Web.Security.Interfaces;
using Newtonsoft.Json;

namespace ArturRios.Common.Web.Security.Middleware;

public class JwtMiddleware(
    RequestDelegate next,
    SettingsProvider settings,
    IAuthenticationProvider authProvider,
    JwtTokenConfiguration tokenConfig) : WebApiMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (!SkipRoute(context.Request.Path.Value ?? string.Empty))
        {
            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() is null)
            {
                var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? "";

                var jwtToken = JwtToken.FromToken(token, tokenConfig.Secret);
                var isValid = jwtToken.IsTokenValidAsync().GetAwaiter().GetResult();

                string authError;

                if (isValid)
                {
                    var userId = jwtToken.GetUserId();

                    if (userId.HasValue)
                    {
                        var authenticatedUser = authProvider.GetAuthenticatedUserById(userId.Value);

                        if (authenticatedUser is not null)
                        {
                            context.Items["User"] = authenticatedUser;

                            return;
                        }

                        authError = "User not found";
                    }
                    else
                    {
                        authError = "Could no retrieve user id from token";
                    }
                }
                else
                {
                    authError = "Invalid token";
                }

                var output = ProcessOutput.New
                    .WithError(authError);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var payload = JsonConvert.SerializeObject(output);

                await context.Response.WriteAsync(payload);

                return;
            }
        }

        await next(context);
    }

    private bool SkipRoute(string path)
    {
        return settings.GetBool(AppSettingsKeys.SwaggerEnabled) is true &&
               path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase);
    }
}
