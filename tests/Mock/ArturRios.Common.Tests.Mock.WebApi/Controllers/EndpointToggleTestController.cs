using ArturRios.Common.Attributes;
using ArturRios.Common.Attributes.EndpointToggle;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Tests.Mock.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EndpointToggleTestController : BaseController
{
    [HttpGet]
    [Route("Enabled")]
    [EndpointToggle]
    public ActionResult<WebApiOutput<string>> Enabled()
    {
        WebApiOutput<string> result = new("Hello world!", ["Endpoint test controller is on..."], true, HttpStatusCodes.Ok);

        return Resolve(result);
    }

    [HttpGet]
    [Route("Disabled")]
    [EndpointToggle(isEnabled: false)]
    public ActionResult<WebApiOutput<string>> Disabled()
    {
        WebApiOutput<string> result = new("Hello world!", ["Endpoint test controller is on..."], true, HttpStatusCodes.Ok);

        return Resolve(result);
    }

    [HttpGet]
    [Route("DisabledByAppSettings")]
    [EndpointToggle(configurationSource: ConfigurationSourceType.AppSettings)]
    public ActionResult<WebApiOutput<string>> DisabledByAppSettings()
    {
        WebApiOutput<string> result = new("Hello world!", ["Endpoint test controller is on..."], true, HttpStatusCodes.Ok);

        return Resolve(result);
    }
}
