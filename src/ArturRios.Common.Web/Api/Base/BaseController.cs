// ReSharper disable UnusedMember.Global
// Reason: This is a base controller class meant to be used in other projects

using ArturRios.Common.Output;
using ArturRios.Common.Web.Api.Output;
using ArturRios.Common.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Web.Api.Base;

public class BaseController : Controller
{
    protected static ActionResult<WebApiOutput<T>> Resolve<T>(WebApiOutput<T> webApiOutput)
        => new ObjectResult(webApiOutput) { StatusCode = webApiOutput.GetHttpStatusCode() };

    public static ActionResult<WebApiOutput<PaginatedOutput<T>>> Resolve<T>(PaginatedOutput<T> paginatedOutput,
        int? statusCode = null)
    {
        var output = WebApiOutput<PaginatedOutput<T>>.New
            .WithData(paginatedOutput)
            .WithErrors(paginatedOutput.Errors)
            .WithMessages(paginatedOutput.Messages);

        var httpStatusCode = statusCode ?? GetDefaultStatusCode(paginatedOutput.Success);

        output.WithHttpStatusCode(httpStatusCode);

        return Resolve(output);
    }

    public static ActionResult<WebApiOutput<T?>> Resolve<T>(DataOutput<T?> dataOutput, int? statusCode = null)
    {
        var output = WebApiOutput<T?>.New
            .WithData(dataOutput.Data)
            .WithErrors(dataOutput.Errors)
            .WithMessages(dataOutput.Messages);

        var httpStatusCode = statusCode ?? GetDefaultStatusCode(dataOutput.Success);

        output.WithHttpStatusCode(httpStatusCode);

        return Resolve(output);
    }

    public static ActionResult<WebApiOutput<object?>> Resolve(ProcessOutput processOutput, int? statusCode = null)
    {
        var output = WebApiOutput<object?>.New
            .WithData(null)
            .WithErrors(processOutput.Errors)
            .WithMessages(processOutput.Messages);

        var httpStatusCode = statusCode ?? GetDefaultStatusCode(processOutput.Success);

        output.WithHttpStatusCode(httpStatusCode);

        return Resolve(output);
    }

    private static int GetDefaultStatusCode(bool success) => success ? HttpStatusCodes.Ok : HttpStatusCodes.BadRequest;
}
