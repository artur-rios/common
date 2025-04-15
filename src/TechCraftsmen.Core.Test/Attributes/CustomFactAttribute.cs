namespace TechCraftsmen.Core.Test.Attributes;

public class CustomFactAttribute : Xunit.FactAttribute
{
    public string Type { get; private set; }
    public string[]? Environments { get; private set; }
        
    protected CustomFactAttribute(string type = "Unit", string[]? environments = null)
    {
        Type = type;
        Environments = environments;

        if (environments is null)
        {
            return;
        }

        var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var hasEnvironment = environments.FirstOrDefault(environment => environment == currentEnvironment);

        if (string.IsNullOrWhiteSpace(hasEnvironment))
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Skip = $"Test can't run on {currentEnvironment}";
        }
    }
}
