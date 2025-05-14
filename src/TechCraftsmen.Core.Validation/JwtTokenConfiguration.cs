namespace TechCraftsmen.Core.Validation;

public record JwtTokenConfiguration(double ExpirationInSeconds, string Issuer, string Audience, string Secret)
{
    // Necessary for dynamic dependency injection
    public JwtTokenConfiguration() : this(0, string.Empty, string.Empty, string.Empty) { }
}
