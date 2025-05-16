namespace TechCraftsmen.Core.WebApi.Security.Records;

// ReSharper disable once ClassNeverInstantiated.Global
// Reason: This record is meant to be used in other projects
public record Credentials(string Email, string Password)
{
    // ReSharper disable once UnusedMember.Global
    // Reason: Necessary for dynamic dependency injection
    public Credentials() : this(string.Empty, string.Empty) { }
}
