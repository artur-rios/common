using Newtonsoft.Json;
using TechCraftsmen.Core.Output;

namespace TechCraftsmen.Core.WebApi;

public class ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
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
        _logger.LogError("Error: {error}", exception.Message);
        _logger.LogError("Stack: {stack}", exception.StackTrace);

        if (exception.InnerException is not null)
        {
            _logger.LogError("Inner exception on request: {message}", exception.InnerException.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = HttpStatusCodes.InternalServerError;

        var output = new DataOutput<string>(string.Empty, ["Internal server error, please try again later"], false);
        
        await context.Response.WriteAsync(JsonConvert.SerializeObject(output));
    }
}
