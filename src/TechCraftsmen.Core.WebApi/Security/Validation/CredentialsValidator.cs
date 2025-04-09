using FluentValidation;
using TechCraftsmen.Core.WebApi.Security.Records;

namespace TechCraftsmen.Core.WebApi.Security.Validation;

public class CredentialsValidator : AbstractValidator<Credentials>
{
    public CredentialsValidator()
    {
        RuleFor(authCredentials => authCredentials.Email).NotEmpty();
        RuleFor(authCredentials => authCredentials.Password).NotEmpty();
    }
}