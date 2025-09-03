using ArturRios.Common.WebApi;
using ArturRios.Common.WebApi.Client;

namespace ArturRios.Common.Tests.Mock.WebApi.Client.Routes;

public class TestRoute(HttpClient httpClient) : BaseWebApiClientRoute(httpClient)
{
    public override string BaseUrl => "/Test";

    public async Task<WebApiOutput<string>?> HelloWorld()
    {
        return await GetAsync<string>($"{BaseUrl}/HelloWorld");
    }
}
