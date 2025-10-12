using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Aws.Tests.WebApi;

public class TestController : BaseController
{
    [HttpGet]
    public ActionResult<WebApiOutput<string>> HelloWorld()
    {
        var result = WebApiOutput<string>.New
            .WithData("Hello world!")
            .WithMessage("Test controller is on...")
            .WithHttpStatusCode(HttpStatusCodes.Ok);

        return Resolve(result);
    }
}
