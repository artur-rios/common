namespace ArturRios.Common.Web.Security.Records;

public record Authentication(string? Token, bool Valid, string CreatedAt, string Expiration);
