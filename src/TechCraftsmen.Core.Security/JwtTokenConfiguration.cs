namespace TechCraftsmen.Core.Util;

public record JwtTokenConfiguration(double ExpirationInSeconds, string Issuer, string Audience, string Secret)
{
    // ReSharper disable once UnusedMember.Global
    // Reason: Necessary for dynamic dependency injection
    public JwtTokenConfiguration() : this(0, string.Empty, string.Empty, string.Empty) { }
}
