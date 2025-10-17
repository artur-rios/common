// ReSharper disable UnusedMember.Global
// Reason: This is a base controller class meant to be used in other projects

using ArturRios.Common.Output;
using ArturRios.Common.Web.Api.Output;
using ArturRios.Common.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Web.Api.Base;

public class BaseController : Controller
{
    protected static ActionResult<WebApiOutput<T>> Resolve<T>(WebApiOutput<T> webApiOutput) =>
        webApiOutput.ToObjectResult();

    public static ActionResult<WebApiOutput<T>> Resolve<T>(DataOutput<T?> dataOutput)
    {
        return WebApiOutput<T?>.New
            .WithData(dataOutput.Data)
            .WithErrors(dataOutput.Errors)
            .WithMessages(dataOutput.Messages)
            .WithHttpStatusCode(GetStatusCode(dataOutput.Success))
            .ToObjectResult();
    }

    public static ActionResult<WebApiOutput<object?>> Resolve(ProcessOutput processOutput)
    {
        return WebApiOutput<object?>.New
            .WithData(null)
            .WithErrors(processOutput.Errors)
            .WithMessages(processOutput.Messages)
            .WithHttpStatusCode(GetStatusCode(processOutput.Success))
            .ToObjectResult();
    }

    private static int GetStatusCode(bool success) => success ? HttpStatusCodes.Ok : HttpStatusCodes.BadRequest;
}
