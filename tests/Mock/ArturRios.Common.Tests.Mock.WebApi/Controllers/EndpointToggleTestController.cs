using ArturRios.Common.Attributes.EndpointToggle;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Output;
using ArturRios.Common.Web.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Tests.Mock.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EndpointToggleTestController : Controller
{
    [HttpGet]
    [Route("Enabled")]
    [EndpointToggle]
    public ActionResult<DataOutput<string?>> Enabled()
    {
        var result = DataOutput<string?>.New
            .WithData("Hello world!")
            .WithMessage("Endpoint test controller is on...");

        return ResponseResolver.Resolve(result);
    }

    [HttpGet]
    [Route("Disabled")]
    [EndpointToggle(false)]
    public ActionResult<DataOutput<string?>> Disabled()
    {
        var result = DataOutput<string?>.New
            .WithData("Hello world!")
            .WithMessage("Endpoint test controller is on...");

        return ResponseResolver.Resolve(result);
    }

    [HttpGet]
    [Route("DisabledByAppSettings")]
    [EndpointToggle(ConfigurationSourceType.AppSettings)]
    public ActionResult<DataOutput<string?>> DisabledByAppSettings()
    {
        var result = DataOutput<string?>.New
            .WithData("Hello world!")
            .WithMessage("Endpoint test controller is on...");

        return ResponseResolver.Resolve(result);
    }
}
