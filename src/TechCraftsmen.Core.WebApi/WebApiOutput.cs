using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.Core.Extensions;
using TechCraftsmen.Core.Output;

namespace TechCraftsmen.Core.WebApi;

public class WebApiOutput<T> : DataOutput<T>
{
    private readonly int _httpStatusCode;

    // Necessary for json serialization
    public WebApiOutput()
    {
    }

    public WebApiOutput(T? data, string[] messages, bool success, int httpStatusCode) : base(data, messages, success)
    {
        ValidateStatusCode(httpStatusCode);

        _httpStatusCode = httpStatusCode;
    }

    public WebApiOutput(DataOutput<T> dataOutput, int httpStatusCode) : base(dataOutput.Data, dataOutput.Messages,
        dataOutput.Success)
    {
        ValidateStatusCode(httpStatusCode);

        _httpStatusCode = httpStatusCode;
    }

    public WebApiOutput(ProcessOutput processOutput, T dataOutput, int httpStatusCode) : base(
        dataOutput,
        processOutput.Success ? processOutput.Errors.ToArray() : ["Process executed with no errors"],
        processOutput.Success)
    {
        ValidateStatusCode(httpStatusCode);

        _httpStatusCode = httpStatusCode;
    }


    public ObjectResult ToObjectResult()
    {
        return new ObjectResult(this) { StatusCode = _httpStatusCode };
    }

    private static void ValidateStatusCode(int httpStatusCode)
    {
        if (httpStatusCode.NotIn(HttpStatusCodes.All))
        {
            throw new ArgumentException("Unsupported status code passed to constructor");
        }
    }
}
