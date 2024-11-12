using TechCraftsmen.Core.Output;
using TechCraftsmen.Core.WebApi.Security.Records;

namespace TechCraftsmen.Core.WebApi.Security.Interfaces;

public interface IAuthenticationService
{
    Authentication AuthenticateUser(Credentials credentials);
    DataOutput<AuthenticatedUser> ValidateTokenAndGetUser(string token);
}
