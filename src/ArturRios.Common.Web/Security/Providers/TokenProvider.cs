using ArturRios.Common.Security;
using ArturRios.Common.Web.Security.Extensions;
using ArturRios.Common.Web.Security.Records;

namespace ArturRios.Common.Web.Security.Providers;

public class TokenProvider
{
    public Authentication Provide(AuthenticatedUser user, JwtTokenConfiguration tokenConfiguration)
    {
        var jwtToken = JwtToken.FromClaims(user.ToTokenClaims(), tokenConfiguration);

        return new Authentication(jwtToken.Token, true, jwtToken.CreatedAt, jwtToken.Expiration);
    }
}
