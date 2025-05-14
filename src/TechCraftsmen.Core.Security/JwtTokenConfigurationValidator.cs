using FluentValidation;

namespace TechCraftsmen.Core.Security;

public class JwtTokenConfigurationValidator : AbstractValidator<JwtTokenConfiguration>
{
    public JwtTokenConfigurationValidator()
    {
        RuleFor(config => config.Audience).NotEmpty();
        RuleFor(config => config.Issuer).NotEmpty();
        RuleFor(config => config.ExpirationInSeconds).NotEmpty().GreaterThan(0);
        RuleFor(config => config.Secret).NotEmpty();
    }
}
