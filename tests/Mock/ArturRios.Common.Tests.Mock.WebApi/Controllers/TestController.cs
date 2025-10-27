using ArturRios.Common.Output;
using ArturRios.Common.Web.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ILogger = ArturRios.Common.Logging.Interfaces.ILogger;

namespace ArturRios.Common.Tests.Mock.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(ILogger logger) : Controller
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

        logger.Trace("Test trace log");
        logger.Debug("Test debug log");
        logger.Info("Test info log");
        logger.Warn("Test warn log");
        logger.Error("Test error log");
        logger.Exception(new Exception("Test exception log"));
        logger.Critical("Test critical log");
        logger.Fatal("Test fatal log");

        return ResponseResolver.Resolve(result);
    }

    [HttpGet]
    [Route("Exception")]
    public ActionResult ThrowException() => throw new Exception("Test exception");
}
