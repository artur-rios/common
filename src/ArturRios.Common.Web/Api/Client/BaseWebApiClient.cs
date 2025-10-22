using ArturRios.Common.Web.Http;

namespace ArturRios.Common.Web.Api.Client;

public abstract class BaseWebApiClient
{
    protected readonly HttpGateway Gateway;

    protected BaseWebApiClient(HttpClient httpClient)
    {
        Gateway = new HttpGateway(httpClient);

        SetRoutes();
    }

    protected BaseWebApiClient(string baseUrl)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

        Gateway = new HttpGateway(httpClient);

        SetRoutes();
    }

    protected abstract void SetRoutes();
}
