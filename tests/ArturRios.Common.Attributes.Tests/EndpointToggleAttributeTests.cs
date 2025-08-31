using System.Net;
using ArturRios.Common.Attributes.EndpointToggle;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Test;
using ArturRios.Common.Tests.Mock.WebApi;

namespace ArturRios.Common.Attributes.Tests;

public class EndpointToggleAttributeTests(EnvironmentType environment = EnvironmentType.Local) : WebApiTest<Program>(environment)
{
    private const string TestRoute = "/EndpointToggleTest";

    [Fact]
    public async Task EndpointShouldBe_Enabled()
    {
        var output = await GetAsync<string>($"{TestRoute}/Enabled", HttpStatusCode.OK);

        Assert.NotNull(output);
        Assert.Equal("Hello world!", output.Data);
        Assert.Equal("Endpoint test controller is on...", output.Messages.First());
    }

    [Fact]
    public async Task EndpointShouldBe_Disabled()
    {
        var output = await GetAsync<string>($"{TestRoute}/Disabled", EndpointToggleAttribute.DefaultDisabledStatusCode);

        Assert.NotNull(output);
        Assert.Null(output.Data);
        Assert.Equal(EndpointToggleAttribute.DefaultDisabledMessage, output.Messages.First());
    }

    [Fact]
    public async Task EndpointShouldBe_DisabledByAppSettings()
    {
        var output = await GetAsync<string>($"{TestRoute}/DisabledByAppSettings", EndpointToggleAttribute.DefaultDisabledStatusCode);

        Assert.NotNull(output);
        Assert.Null(output.Data);
        Assert.Equal(EndpointToggleAttribute.DefaultDisabledMessage, output.Messages.First());
    }
}
