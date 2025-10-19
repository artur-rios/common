using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ArturRios.Common.Security;

public class JwtToken
{
    private readonly JwtSecurityTokenHandler _handler = new();

    private readonly byte[] _key;
    private readonly SecurityToken? _securityToken;

    public static JwtToken FromToken(string token, string secret = "") => new(token, secret);
    public static JwtToken FromClaims(Dictionary<string, string> claims, JwtTokenConfiguration configuration) => new(claims, configuration);

    private JwtToken(string token, string secret = "")
    {
        Token = token;

        if (_handler.CanReadToken(Token))
        {
            _securityToken = _handler.ReadToken(Token);
        }

        _key = string.IsNullOrWhiteSpace(secret) ? [] : Encoding.ASCII.GetBytes(secret);
    }

    private JwtToken(Dictionary<string, string> claims, JwtTokenConfiguration configuration)
    {
        _key = string.IsNullOrWhiteSpace(configuration.Secret) ? [] : Encoding.ASCII.GetBytes(configuration.Secret);

        var claimsList = claims.Select(c => new Claim(c.Key, c.Value)).ToList();

        ClaimsIdentity identity = new(claimsList);

        var creationDate = DateTime.Now;
        var expirationDate = creationDate + TimeSpan.FromSeconds(configuration.ExpirationInSeconds);

        JwtSecurityTokenHandler handler = new();
        _securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = configuration.Issuer,
            Audience = configuration.Audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
            Subject = identity,
            NotBefore = creationDate,
            Expires = expirationDate
        });

        Token = handler.WriteToken(_securityToken);
        CreatedAt = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
        Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string Token { get; }
    public string CreatedAt { get; } = string.Empty;
    public string Expiration { get; } = string.Empty;

    public int? GetUserId()
    {
        if (_securityToken is null)
        {
            return null;
        }

        var jwtToken = _securityToken as JwtSecurityToken;

        return int.Parse(jwtToken!.Claims.First(x => x.Type == "id").Value);
    }

    public async Task<bool> IsTokenValidAsync()
    {
        var output = await _handler.ValidateTokenAsync(Token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

        return output.IsValid;
    }
}
