using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.Core.Output;

namespace TechCraftsmen.Core.WebApi;

public class BaseController : Controller
{
    public static ActionResult<WebApiOutput<T>> Resolve<T>(WebApiOutput<T> webApiOutput)
    {
        return webApiOutput.ToObjectResult();
    }
    
    public static ActionResult<WebApiOutput<T?>> Resolve<T>(DataOutput<T?> dataOutput)
    {
        var statusCode = dataOutput.Success ? HttpStatusCodes.Ok : HttpStatusCodes.BadRequest;
        var webApiOutput = new WebApiOutput<T?>(dataOutput, statusCode);
        
        return webApiOutput.ToObjectResult();
    }
}
