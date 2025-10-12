using ArturRios.Common.Extensions;
using ArturRios.Common.WebApi.Security.Records;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArturRios.Common.WebApi.Security.Filters;

public class RoleRequirementFilter(params int[] authorizedRoles) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as AuthenticatedUser;

        var authorized = false;

        if (user is not null)
        {
            authorized = user.Role.In(authorizedRoles);
        }

        if (authorized)
        {
            return;
        }

        context.Result = WebApiOutput<string>.New
            .WithData("Forbidden")
            .WithError("You do not have permission to access this resource")
            .WithHttpStatusCode(HttpStatusCodes.Forbidden)
            .ToObjectResult();
    }
}
