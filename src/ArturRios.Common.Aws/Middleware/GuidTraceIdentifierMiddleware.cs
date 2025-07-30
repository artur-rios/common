using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Aws.Middleware;

public class GuidTraceIdentifierMiddleware(RequestDelegate next, ILogger<GuidTraceIdentifierMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var bodyString = "<payload not available>";
        var lambdaContext = context.Items[AbstractAspNetCoreFunction.LAMBDA_CONTEXT] as ILambdaContext;

        if (System.Environment.GetEnvironmentVariable("DISABLE_API_REQUEST_LOG") is null)
        {
            try
            {
                context.Request.EnableBuffering();
                var reader = new StreamReader(context.Request.Body);
                bodyString = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to read request body: {ExMessage}", ex.Message);
            }
        }
        else
        {
            bodyString = "<payload logging disabled>";
        }

        context.TraceIdentifier = lambdaContext?.AwsRequestId ?? Guid.NewGuid().ToString("N");
        logger.LogInformation("Starting request with trace identifier: {TraceIdentifier} | Payload: {Payload}", context.TraceIdentifier, bodyString);

        await next(context);
    }
}
