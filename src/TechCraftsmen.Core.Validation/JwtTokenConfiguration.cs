namespace TechCraftsmen.Core.Validation;

public record JwtTokenConfiguration(double ExpirationInSeconds, string Issuer, string Audience, string Secret);
