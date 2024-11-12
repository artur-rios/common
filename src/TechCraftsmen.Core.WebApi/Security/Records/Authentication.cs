namespace TechCraftsmen.Core.WebApi.Security.Records;

public record Authentication(string? Token, bool Valid, string CreatedAt, string Expiration);
