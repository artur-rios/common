using System.Net;
using ArturRios.Common.Attributes.EndpointToggle;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Output;
using ArturRios.Common.Test;
using ArturRios.Common.Tests.Mock.WebApi;

namespace ArturRios.Common.Attributes.Tests;

public class EndpointToggleAttributeTests(EnvironmentType environment = EnvironmentType.Local)
    : WebApiTest<Program>(environment)
{
    private const string TestRoute = "/EndpointToggleTest";

    [Fact]
    public async Task EndpointShouldBe_Enabled()
    {
        var output = await Gateway.GetAsync<DataOutput<string>>($"{TestRoute}/Enabled");

        Assert.Equal(HttpStatusCode.OK, output.StatusCode);
        Assert.NotNull(output.Body);
        Assert.Equal("Hello world!", output.Body.Data);
        Assert.Equal("Endpoint test controller is on...", output.Body.Messages.First());
    }

    [Fact]
    public async Task EndpointShouldBe_Disabled()
    {
        var output = await Gateway.GetAsync<DataOutput<string>>($"{TestRoute}/Disabled");

        Assert.Equal(EndpointToggleAttribute.DefaultDisabledStatusCode, output.StatusCode);
        Assert.NotNull(output.Body);
        Assert.Equal(EndpointToggleAttribute.DefaultDisabledMessage, output.Body.Messages.First());
    }

    [Fact]
    public async Task EndpointShouldBe_DisabledByAppSettings()
    {
        var output = await Gateway.GetAsync<DataOutput<string>>($"{TestRoute}/DisabledByAppSettings");

        Assert.Equal(EndpointToggleAttribute.DefaultDisabledStatusCode, output.StatusCode);
        Assert.NotNull(output.Body);
        Assert.Equal(EndpointToggleAttribute.DefaultDisabledMessage, output.Body.Messages.First());
    }
}
