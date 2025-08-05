using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.WebApi;

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
        processOutput.Success ? ["Process executed with no errors"] : processOutput.Errors.ToArray(),
        processOutput.Success)
    {
        ValidateStatusCode(httpStatusCode);

        _httpStatusCode = httpStatusCode;
    }


    public ObjectResult ToObjectResult() => new(this) { StatusCode = _httpStatusCode };

    private static void ValidateStatusCode(int httpStatusCode)
    {
        if (httpStatusCode.NotIn(HttpStatusCodes.All))
        {
            throw new ArgumentException("Unsupported status code passed to constructor");
        }
    }
}
