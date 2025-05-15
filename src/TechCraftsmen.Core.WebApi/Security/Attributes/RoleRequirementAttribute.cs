using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.Core.WebApi.Security.Filters;

namespace TechCraftsmen.Core.WebApi.Security.Attributes;

// ReSharper disable once UnusedType.Global
// Reason: This is a custom attribute meant to be used in other projects
public class RoleRequirementAttribute : TypeFilterAttribute
{
    public RoleRequirementAttribute(params int[] authorizedRoles) : base(typeof(RoleRequirementFilter))
    {
        object[] arguments = [authorizedRoles];

        Arguments = arguments;
    }
}
