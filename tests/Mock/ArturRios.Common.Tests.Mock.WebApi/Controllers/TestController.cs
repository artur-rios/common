using ArturRios.Common.Attributes;
using ArturRios.Common.Configuration;
using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Tests.Mock.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : BaseController
{
    [HttpGet]
    [Route("HelloWorld")]
    [EndpointToggle]
    public ActionResult<WebApiOutput<string>> HelloWorld()
    {
        WebApiOutput<string> result = new("Hello world!", ["Test controller is on..."], true, HttpStatusCodes.Ok);

        return Resolve(result);
    }

    [HttpGet]
    [Route("Disabled")]
    [EndpointToggle(false)]
    public ActionResult<WebApiOutput<string>> DisabledHelloWorld()
    {
        WebApiOutput<string> result = new("Hello world!", ["Test controller is on..."], true, HttpStatusCodes.Ok);

        return Resolve(result);
    }

    [HttpGet]
    [Route("DisabledByAppSettings")]
    [EndpointToggle(ConfigurationSourceType.AppSettings)]
    public ActionResult<WebApiOutput<string>> DisabledViaAppSettingsHelloWorld()
    {
        WebApiOutput<string> result = new("Hello world!", ["Test controller is on..."], true, HttpStatusCodes.Ok);

        return Resolve(result);
    }
}
