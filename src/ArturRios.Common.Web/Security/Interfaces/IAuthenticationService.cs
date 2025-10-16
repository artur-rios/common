using ArturRios.Common.Output;
using ArturRios.Common.Web.Security.Records;

namespace ArturRios.Common.Web.Security.Interfaces;

public interface IAuthenticationService
{
    // ReSharper disable once UnusedMember.Global
    // This method is meant to be used in other projects
    DataOutput<Authentication> AuthenticateUser(Credentials credentials);
    DataOutput<AuthenticatedUser> ValidateTokenAndGetUser(string token);
}
