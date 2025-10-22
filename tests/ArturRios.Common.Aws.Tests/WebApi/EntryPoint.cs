using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ArturRios.Common.Aws.Tests.WebApi;

public class EntryPoint : APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder) => builder.UseStartup<Startup>();

    protected override void Init(IHostBuilder builder)
    {
    }
}
