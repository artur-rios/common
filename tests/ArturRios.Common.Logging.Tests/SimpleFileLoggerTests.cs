using System.Net;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Output;
using ArturRios.Common.Test;
using ArturRios.Common.Tests.Mock.WebApi;

namespace ArturRios.Common.Logging.Tests;

public class SimpleFileLoggerTests(EnvironmentType environment = EnvironmentType.Local)
    : WebApiTest<Program>(environment)
{
    private const string TestRoute = "/Test";

    [Fact]
    public async Task Should_LogOnFile()
    {
        var output = await Gateway.GetAsync<DataOutput<string>>($"{TestRoute}/HelloWorld");

        Assert.Equal(HttpStatusCode.OK, output.StatusCode);
    }
}
