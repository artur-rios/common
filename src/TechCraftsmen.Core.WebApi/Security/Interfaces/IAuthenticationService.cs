using TechCraftsmen.Core.Output;
using TechCraftsmen.Core.WebApi.Security.Records;

namespace TechCraftsmen.Core.WebApi.Security.Interfaces;

public interface IAuthenticationService
{
    // ReSharper disable once UnusedMember.Global
    // This method is meant to be used in other projects
    DataOutput<Authentication> AuthenticateUser(Credentials credentials);
    DataOutput<AuthenticatedUser> ValidateTokenAndGetUser(string token);
}
