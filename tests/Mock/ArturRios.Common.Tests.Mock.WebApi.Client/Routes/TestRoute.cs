using ArturRios.Common.Web;
using ArturRios.Common.Web.Api.Client;
using ArturRios.Common.Web.Api.Output;

namespace ArturRios.Common.Tests.Mock.WebApi.Client.Routes;

public class TestRoute(HttpClient httpClient) : BaseWebApiClientRoute(httpClient)
{
    public override string BaseUrl => "/Test";

    public async Task<WebApiOutput<string>?> HelloWorld()
    {
        return await GetAsync<string>($"{BaseUrl}/HelloWorld");
    }
}
