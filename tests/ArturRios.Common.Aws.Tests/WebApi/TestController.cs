using ArturRios.Common.Output;
using ArturRios.Common.Web.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ArturRios.Common.Aws.Tests.WebApi;

public class TestController : Controller
{
    [HttpGet]
    public ActionResult<DataOutput<string?>> HelloWorld()
    {
        var result = DataOutput<string?>.New
            .WithData("Hello world!")
            .WithMessage("Test controller is on...");

        return ResponseResolver.Resolve(result);
    }
}
