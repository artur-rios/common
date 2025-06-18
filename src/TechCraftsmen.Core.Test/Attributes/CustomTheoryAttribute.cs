// ReSharper disable VirtualMemberCallInConstructor
// Reason: call needed
using TechCraftsmen.Core.Environment;

namespace TechCraftsmen.Core.Test.Attributes;

public class CustomTheoryAttribute : Xunit.TheoryAttribute
{
    // ReSharper disable once MemberCanBePrivate.Global
    // Reason: This property is used by the test framework to determine if the test should be skipped
    public EnvironmentType[]? Environments { get; }

    protected CustomTheoryAttribute(EnvironmentType[]? environments = null, bool skip = false)
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
        
        if (skip)
        {
            Skip = "Condition to skip matched";
        }
    }
}
