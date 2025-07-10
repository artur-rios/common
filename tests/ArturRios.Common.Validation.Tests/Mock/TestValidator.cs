using FluentValidation;

namespace ArturRios.Common.Validation.Tests.Mock;

public class TestValidator : FluentValidator<ModelTest>
{
    public TestValidator()
    {
        RuleFor(test => test.Name).NotEmpty();
        RuleFor(test => test.Email).NotEmpty().EmailAddress();
        RuleFor(test => test.Number).GreaterThan(0);
    }
}
