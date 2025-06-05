using Microsoft.AspNetCore.Mvc.Filters;
using TechCraftsmen.Core.Extensions;
using TechCraftsmen.Core.WebApi.Security.Records;

namespace TechCraftsmen.Core.WebApi.Security.Filters;

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
        
        var webApiOutput = new WebApiOutput<string>("Forbidden", ["You do not have permission to access this resource"], false, HttpStatusCodes.Forbidden);
            
        context.Result = webApiOutput.ToObjectResult();
    }
}
