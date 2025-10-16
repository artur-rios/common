using ArturRios.Common.Attributes.EndpointToggle;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Web;
using ArturRios.Common.Web.Api.Base;
using ArturRios.Common.Web.Api.Output;
using ArturRios.Common.Web.Http;
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
        var result = WebApiOutput<string>.New
            .WithData("Hello world!")
            .WithMessage("Endpoint test controller is on...")
            .WithHttpStatusCode(HttpStatusCodes.Ok);

        return Resolve(result);
    }

    [HttpGet]
    [Route("Disabled")]
    [EndpointToggle(isEnabled: false)]
    public ActionResult<WebApiOutput<string>> Disabled()
    {
        var result = WebApiOutput<string>.New
            .WithData("Hello world!")
            .WithMessage("Endpoint test controller is on...")
            .WithHttpStatusCode(HttpStatusCodes.Ok);

        return Resolve(result);
    }

    [HttpGet]
    [Route("DisabledByAppSettings")]
    [EndpointToggle(configurationSource: ConfigurationSourceType.AppSettings)]
    public ActionResult<WebApiOutput<string>> DisabledByAppSettings()
    {
        var result = WebApiOutput<string>.New
            .WithData("Hello world!")
            .WithMessage("Endpoint test controller is on...")
            .WithHttpStatusCode(HttpStatusCodes.Ok);

        return Resolve(result);
    }
}
