using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Test;
using ArturRios.Common.Tests.Mock.WebApi;
using ArturRios.Common.Tests.Mock.WebApi.Client;

namespace ArturRios.Common.WebApi.Tests;

public class WebApiClientTests : WebApiTest<Program>
{
    private readonly WebApiClient _webApiClient;

    public WebApiClientTests(EnvironmentType environment = EnvironmentType.Local) : base(environment)
    {
        _webApiClient = new WebApiClient(Gateway.Client);
    }

    [Fact]
    public async Task Should_DoHealthCheck()
    {
        var output = await _webApiClient.Test.HelloWorld();

        Assert.NotNull(output.Body?.Data);
        Assert.Equal("Hello world!", output.Body.Data);
        Assert.Equal("Test controller is on...", output.Body.Messages.First());
    }
}
