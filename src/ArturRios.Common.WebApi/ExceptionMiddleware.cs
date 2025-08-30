// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This middleware is meant to be used in other projects

using ArturRios.Common.Output;
using Newtonsoft.Json;

namespace ArturRios.Common.WebApi;

public class ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) : WebApiMiddleware
{
    private readonly ILogger _logger = loggerFactory.CreateLogger(typeof(ExceptionMiddleware));

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        string[] messages = ["Internal server error, please try again later"];

        if (exception is CustomException customException)
        {
            messages = customException.Messages;
        }

        _logger.LogError("Error: {error}", exception.Message);
        _logger.LogError("Stack: {stack}", exception.StackTrace);

        foreach (var message in messages)
        {
            _logger.LogError("Message: {message}", message);
        }

        if (exception.InnerException is not null)
        {
            _logger.LogError("Inner exception on request: {message}", exception.InnerException.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = HttpStatusCodes.InternalServerError;

        var output = new DataOutput<string>(string.Empty, messages, false);

        await context.Response.WriteAsync(JsonConvert.SerializeObject(output));
    }
}
