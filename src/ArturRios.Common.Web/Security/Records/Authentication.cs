// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global
// Reason: This record is meant to be used in other projects

namespace ArturRios.Common.Web.Security.Records;

public record Authentication(string? Token, bool Valid, string CreatedAt, string Expiration);
