using ArturRios.Common.Test.Attributes;
using ArturRios.Common.Validation.Tests.Mock;
using Xunit;

namespace ArturRios.Common.Validation.Tests;

public class FluentValidatorTests
{
    [UnitFact]
    public void Should_ValidateAndReturnErrors()
    {
        var model = new ModelTest { Name = "", Email = "test@mail.com", Number = 42 };
        var validator = new TestValidator();
        var validationErrors = validator.ValidateAndReturnErrors(model);

        Assert.NotEmpty(validationErrors);
        Assert.Equal("Name must not be empty", validationErrors.First());
    }
}