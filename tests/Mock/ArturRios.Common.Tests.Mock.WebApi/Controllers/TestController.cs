using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Tests.Mock.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : BaseController
{
    [HttpGet]
    [Route("HelloWorld")]
    public ActionResult<WebApiOutput<string>> HelloWorld()
    {
        var result = WebApiOutput<string>.New
            .WithData("Hello world!")
            .WithMessage("Test controller is on...")
            .WithHttpStatusCode(HttpStatusCodes.Ok);
        return Resolve(result);
    }

    [HttpGet]
    [Route("Exception")]
    public ActionResult<WebApiOutput<string>> ThrowException()
    {
        throw new Exception("Test exception");
    }
}
