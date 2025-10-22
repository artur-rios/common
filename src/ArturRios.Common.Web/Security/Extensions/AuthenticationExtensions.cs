using ArturRios.Common.Web.Security.Records;

namespace ArturRios.Common.Web.Security.Extensions;

public static class AuthenticationExtensions
{
    public static Dictionary<string, string> ToTokenClaims(this AuthenticatedUser authenticatedUser) =>
        new() { { "id", authenticatedUser.Id.ToString() }, { "role", authenticatedUser.Role.ToString() } };
}
