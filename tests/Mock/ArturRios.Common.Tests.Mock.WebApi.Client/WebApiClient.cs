using ArturRios.Common.Tests.Mock.WebApi.Client.Routes;
using ArturRios.Common.Web.Api.Client;

namespace ArturRios.Common.Tests.Mock.WebApi.Client;

public class WebApiClient : BaseWebApiClient
{
    public TestRoute Test { get; private set; } = null!;
    public WebApiClient(HttpClient httpClient) : base(httpClient) { }

    public WebApiClient(string baseUrl) : base(baseUrl) { }

    protected override void SetRoutes()
    {
        Test = new TestRoute(HttpClient);
    }
}
