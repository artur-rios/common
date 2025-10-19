namespace ArturRios.Common.Security;

public record JwtTokenConfiguration(double ExpirationInSeconds, string Issuer, string Audience, string Secret)
{
    public JwtTokenConfiguration() : this(0, string.Empty, string.Empty, string.Empty) { }
}
