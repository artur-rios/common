using ArturRios.Common.Validation;
using ArturRios.Common.WebApi.Security.Records;
using FluentValidation;

namespace ArturRios.Common.WebApi.Security.Validation;

// ReSharper disable once UnusedType.Global
// Reason: This validator is meant to be used in other projects
public class CredentialsValidator : FluentValidator<Credentials>
{
    public CredentialsValidator()
    {
        RuleFor(authCredentials => authCredentials.Email).NotEmpty();
        RuleFor(authCredentials => authCredentials.Password).NotEmpty();
    }
}
