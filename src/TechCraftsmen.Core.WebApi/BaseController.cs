using Microsoft.AspNetCore.Mvc;

namespace TechCraftsmen.Core.WebApi;

public class BaseController : Controller
{
    public static ActionResult<WebApiOutput<T>> Resolve<T>(WebApiOutput<T> webApiOutput)
    {
        return webApiOutput.ToObjectResult();
    }
}
