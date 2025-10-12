using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.WebApi;

public class WebApiOutput<T> : DataOutput<T>
{
    private int _httpStatusCode;

    // Necessary for json serialization
    public WebApiOutput()
    {
    }

    public static new WebApiOutput<T> New => new();

    public new WebApiOutput<T> WithData(T data)
    {
        Data = data;

        return this;
    }

    public new WebApiOutput<T> WithError(string error)
    {
        AddError(error);

        return this;
    }

    public new WebApiOutput<T> WithErrors(IEnumerable<string> errors)
    {
        AddErrors(errors);

        return this;
    }

    public new WebApiOutput<T> WithMessage(string message)
    {
        AddMessage(message);

        return this;
    }

    public new WebApiOutput<T> WithMessages(IEnumerable<string> messages)
    {
        AddMessages(messages);

        return this;
    }

    public WebApiOutput<T> WithHttpStatusCode(int httpStatusCode)
    {
        ValidateStatusCode(httpStatusCode);

        _httpStatusCode = httpStatusCode;

        return this;
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
