using ArturRios.Common.Validation;
using ArturRios.Common.Web.Security.Records;
using FluentValidation;

namespace ArturRios.Common.Web.Security.Validation;

public class CredentialsValidator : FluentValidator<Credentials>
{
    public CredentialsValidator()
    {
        RuleFor(authCredentials => authCredentials.Email).NotEmpty();
        RuleFor(authCredentials => authCredentials.Password).NotEmpty();
    }
}
