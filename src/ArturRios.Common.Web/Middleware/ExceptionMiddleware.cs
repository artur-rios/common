using ArturRios.Common.Output;
using ArturRios.Common.Web.Http;
using Newtonsoft.Json;
using ILogger = ArturRios.Common.Logging.Interfaces.ILogger;

namespace ArturRios.Common.Web.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger logger) : WebApiMiddleware
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (OperationCanceledException oce) when (httpContext.RequestAborted.IsCancellationRequested)
        {
            logger.Debug($"Request was canceled by the client: {oce.Message}");
        }
        catch (TaskCanceledException tce) when (httpContext.RequestAborted.IsCancellationRequested)
        {
            logger.Debug($"Request was canceled by the client (TaskCanceled): {tce.Message}");
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        if (context.RequestAborted.IsCancellationRequested || context.Response.HasStarted)
        {
            logger.Debug("Cannot write error response because the request was aborted or the response has already started.");
            return;
        }

        var messages = new[] { "Internal server error, please try again later" };

        if (exception is CustomException customException)
        {
            messages = customException.Messages;
        }

        logger.Exception(exception);
        logger.Error($"Stack: {exception.StackTrace}");

        foreach (var message in messages)
        {
            logger.Error($"Message: {message}");
        }

        if (exception.InnerException is not null)
        {
            logger.Error($"Inner exception on request: {exception.InnerException.Message}");
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = HttpStatusCodes.InternalServerError;

        var output = DataOutput<string>.New
            .WithData(string.Empty)
            .WithMessages(messages);

        await context.Response.WriteAsync(JsonConvert.SerializeObject(output));
    }
}
