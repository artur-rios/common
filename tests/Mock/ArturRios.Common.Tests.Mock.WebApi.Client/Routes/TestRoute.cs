using ArturRios.Common.Output;
using ArturRios.Common.Web.Api.Client;
using ArturRios.Common.Web.Http;

namespace ArturRios.Common.Tests.Mock.WebApi.Client.Routes;

public class TestRoute(HttpGateway gateway) : BaseWebApiClientRoute(gateway)
{
    public override string BaseUrl => "/Test";

    public async Task<HttpOutput<DataOutput<string?>?>> HelloWorld() =>
        await Gateway.GetAsync<DataOutput<string?>>($"{BaseUrl}/HelloWorld");
}
