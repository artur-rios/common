// ReSharper disable UnusedMember.Global
// Reason: This is a base controller class meant to be used in other projects

using ArturRios.Common.Output;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.WebApi;

public class BaseController : Controller
{
    protected static ActionResult<WebApiOutput<T>> Resolve<T>(WebApiOutput<T> webApiOutput) =>
        webApiOutput.ToObjectResult();

    public static ActionResult<WebApiOutput<T>> Resolve<T>(DataOutput<T?> dataOutput)
    {
        var statusCode = dataOutput.Success ? HttpStatusCodes.Ok : HttpStatusCodes.BadRequest;

        return WebApiOutput<T?>.New
            .WithData(dataOutput.Data)
            .WithHttpStatusCode(statusCode)
            .ToObjectResult();
    }

    public static ActionResult<WebApiOutput<string>> Resolve(ProcessOutput processOutput, string successOutput)
    {
        var statusCode = processOutput.Success ? HttpStatusCodes.Ok : HttpStatusCodes.BadRequest;
        var dataOutput = processOutput.Success ? successOutput : GetFailureOutput(processOutput.Errors.Count);

        return WebApiOutput<string>.New
            .WithData(dataOutput)
            .WithErrors(processOutput.Errors)
            .WithHttpStatusCode(statusCode)
            .ToObjectResult();
    }

    private static string GetFailureOutput(int errorCount) =>
        $"Process executed with {errorCount} error{(errorCount > 1 ? "s" : string.Empty)}";
}
