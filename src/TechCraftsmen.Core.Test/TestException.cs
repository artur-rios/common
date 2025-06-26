namespace TechCraftsmen.Core.Test;

public class TestException(string message, string[]? details = null) : Exception(message)
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // Reason: this property is used to store additional details about the exception and should be accessible for testing purposes
    public string[]? ExceptionDetails { get; set; } = details;
}
