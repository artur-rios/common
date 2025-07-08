// ReSharper disable VirtualMemberCallInConstructor
// Reason: call needed

using ArturRios.Common.Environment;

namespace ArturRios.Common.Test.Attributes;

public class CustomFactAttribute : Xunit.FactAttribute
{
    // ReSharper disable once MemberCanBePrivate.Global
    // Reason: This property is used by the test framework to determine if the test should be skipped
    public EnvironmentType[]? Environments { get; }

    protected CustomFactAttribute(EnvironmentType[]? environments = null, bool skipCondition = false)
    {
        Environments = environments;

        if (environments is null)
        {
            return;
        }

        var currentEnvironment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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
}
