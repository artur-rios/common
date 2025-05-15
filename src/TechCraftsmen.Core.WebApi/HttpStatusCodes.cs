// ReSharper disable MemberCanBePrivate.Global
// Reason: These properties are meant to be used in other projects

namespace TechCraftsmen.Core.WebApi;

public static class HttpStatusCodes
{
    public const int Ok = 200;
    public const int Created = 201;
    public const int NoContent = 204;

    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int Forbidden = 403;
    public const int NotFound = 404;

    public const int InternalServerError = 500;
    public const int NotImplemented = 501;
    public const int BadGateway = 502;

    public static int[] Success => [Ok, Created, NoContent];
    public static int[] ClientError => [BadRequest, Unauthorized, Forbidden, NotFound];
    public static int[] ServerError => [InternalServerError, NotImplemented, BadGateway];
    public static int[] All => [..Success, ..ClientError, ..ServerError];
}
