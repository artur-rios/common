﻿using System.Diagnostics;

namespace ArturRios.Common.Web.Middleware;

public class TraceActivityMiddleware(RequestDelegate next, ILogger<TraceActivityMiddleware> logger) : WebApiMiddleware
{
    private const string TraceParentHeader = "traceparent";

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;

        var createdActivity = false;
        var activity = Activity.Current;

        if (activity == null)
        {
            activity = new Activity("ServerReceive").SetIdFormat(ActivityIdFormat.W3C).Start();
            createdActivity = true;
        }

        var traceId = activity.TraceId.ToString();

        context.TraceIdentifier = traceId;
        context.Items["TraceId"] = traceId;


        var tp = $"00-{activity.TraceId}-{activity.SpanId}-{(activity.Recorded ? "01" : "00")}";

        context.Response.Headers[TraceParentHeader] = tp;

        logger.LogTrace("Started request with TraceId {TraceId}", traceId);

        try
        {
            await next(context);
        }
        finally
        {
            logger.LogTrace("Ending request with TraceId {TraceId}", traceId);

            if (createdActivity && Activity.Current == activity)
            {
                activity.Stop();
            }
        }
    }
}
