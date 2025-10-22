using ArturRios.Common.Tests.Mock.WebApi.Client.Routes;
using ArturRios.Common.Web.Api.Client;

namespace ArturRios.Common.Tests.Mock.WebApi.Client;

public class WebApiClient : BaseWebApiClient
{
    public WebApiClient(HttpClient httpClient) : base(httpClient) { }

    public WebApiClient(string baseUrl) : base(baseUrl) { }
    public TestRoute Test { get; private set; } = null!;

    protected override void SetRoutes() => Test = new TestRoute(Gateway);
}
