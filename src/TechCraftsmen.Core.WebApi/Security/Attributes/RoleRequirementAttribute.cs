using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.Core.WebApi.Security.Filters;

namespace TechCraftsmen.Core.WebApi.Security.Attributes;

public class RoleRequirementAttribute : TypeFilterAttribute
{
    public RoleRequirementAttribute(params int[] authorizedRoles) : base(typeof(RoleRequirementFilter))
    {
        object[] arguments = [authorizedRoles];

        Arguments = arguments;
    }
}
