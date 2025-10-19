using ArturRios.Common.Validation;
using FluentValidation;

namespace ArturRios.Common.Security;

public class JwtTokenConfigurationValidator : FluentValidator<JwtTokenConfiguration>
{
    public JwtTokenConfigurationValidator()
    {
        RuleFor(config => config.Audience).NotEmpty();
        RuleFor(config => config.Issuer).NotEmpty();
        RuleFor(config => config.ExpirationInSeconds).NotEmpty().GreaterThan(0);
        RuleFor(config => config.Secret).NotEmpty();
    }
}
