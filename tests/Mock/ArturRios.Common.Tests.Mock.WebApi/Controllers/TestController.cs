using ArturRios.Common.Logging;
using ArturRios.Common.Output;
using ArturRios.Common.Web.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Tests.Mock.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(SimpleFileLogger logger) : Controller
{
    [HttpGet]
    [Route("HelloWorld")]
    public ActionResult<DataOutput<string?>> HelloWorld()
    {
        var result = DataOutput<string?>.New
            .WithData("Hello world!")
            .WithMessage("Test controller is on...");

        logger.Info("Hello world!");

        return ResponseResolver.Resolve(result);
    }

    [HttpGet]
    [Route("Exception")]
    public ActionResult ThrowException() => throw new Exception("Test exception");
}
