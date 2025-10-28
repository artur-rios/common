using ArturRios.Common.Output;
using ArturRios.Common.Web.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Tests.Mock.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(ILogger<TestController> logger) : Controller
{
    [HttpGet]
    [Route("HelloWorld")]
    public ActionResult<DataOutput<string?>> HelloWorld()
    {
        var result = DataOutput<string?>.New
            .WithData("Hello world!")
            .WithMessage("Test controller is on...");

        return ResponseResolver.Resolve(result);
    }

    [HttpGet]
    [Route("Logs")]
    public ActionResult<DataOutput<string?>> TestLogs()
    {
        var result = DataOutput<string?>.New
            .WithData("Logs tested")
            .WithMessage("Check the application logs for log entries");

        logger.LogTrace("Test trace log");
        logger.LogDebug("Test debug log");
        logger.LogInformation("Test info log");
        logger.LogWarning("Test warn log");
        logger.LogError("Test error log");
        logger.LogCritical("Test critical log");

        return ResponseResolver.Resolve(result);
    }

    [HttpGet]
    [Route("Exception")]
    public ActionResult ThrowException() => throw new Exception("Test exception");
}
