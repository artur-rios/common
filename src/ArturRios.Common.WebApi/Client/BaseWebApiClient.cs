// ReSharper disable VirtualMemberCallInConstructor
// Reason: This is intentional to allow derived classes to set up routes during construction
namespace ArturRios.Common.WebApi.Client;

public abstract class BaseWebApiClient
{
    protected readonly HttpClient HttpClient;

    protected BaseWebApiClient(HttpClient httpClient)
    {
        HttpClient = httpClient;

        SetRoutes();
    }

    protected BaseWebApiClient(string baseUrl)
    {
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        SetRoutes();
    }

    protected abstract void SetRoutes();
}
