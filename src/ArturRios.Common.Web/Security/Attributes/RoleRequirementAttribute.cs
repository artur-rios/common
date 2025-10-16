using ArturRios.Common.Web.Security.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Web.Security.Attributes;

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
