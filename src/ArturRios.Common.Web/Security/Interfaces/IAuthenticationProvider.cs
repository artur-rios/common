using ArturRios.Common.Web.Security.Records;

namespace ArturRios.Common.Web.Security.Interfaces;

public interface IAuthenticationProvider
{
    AuthenticatedUser? GetAuthenticatedUserById(int id);
}
