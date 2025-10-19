using ArturRios.Common.Web.Security.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Web.Security.Attributes;

public class RoleRequirementAttribute : TypeFilterAttribute
{
    public RoleRequirementAttribute(params int[] authorizedRoles) : base(typeof(RoleRequirementFilter))
    {
        object[] arguments = [authorizedRoles];

        Arguments = arguments;
    }
}
