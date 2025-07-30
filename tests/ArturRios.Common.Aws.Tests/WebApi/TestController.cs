using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Aws.Tests.WebApi;

public class TestController : BaseController
{
    [HttpGet]
    public ActionResult<WebApiOutput<string>> HelloWorld()
    {
        WebApiOutput<string> result = new("Hello world!", ["User management Web API is ON"], true, HttpStatusCodes.Ok);

        return Resolve(result);
    }
}
