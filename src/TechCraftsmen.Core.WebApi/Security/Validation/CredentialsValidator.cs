using FluentValidation;
using TechCraftsmen.Core.WebApi.Security.Records;

namespace TechCraftsmen.Core.WebApi.Security.Validation;

// ReSharper disable once UnusedType.Global
// Reason: This validator is meant to be used in other projects
public class CredentialsValidator : AbstractValidator<Credentials>
{
    public CredentialsValidator()
    {
        RuleFor(authCredentials => authCredentials.Email).NotEmpty();
        RuleFor(authCredentials => authCredentials.Password).NotEmpty();
    }
}