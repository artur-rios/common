using ArturRios.Common.Configuration.Enums;
using Xunit;

namespace ArturRios.Common.Test.Attributes;

public class CustomTheoryAttribute : TheoryAttribute
{
    protected CustomTheoryAttribute(EnvironmentType[]? environments = null, bool skipCondition = false)
    {
        Environments = environments;

        if (environments is null)
        {
            return;
        }

        var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var hasEnvironment =
            Environments?.Any(x => x.ToString().Equals(currentEnvironment, StringComparison.OrdinalIgnoreCase)) ?? true;

        if (hasEnvironment)
        {
            Skip = $"Test can't run on {currentEnvironment}";

            return;
        }

        if (skipCondition)
        {
            Skip = "Condition to skip matched";
        }
    }

    public EnvironmentType[]? Environments { get; }
}
