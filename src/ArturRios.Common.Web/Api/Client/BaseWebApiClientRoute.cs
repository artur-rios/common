// ReSharper disable MemberCanBePrivate.Global
// Reason: These methods are intended to be used by derived classes

using ArturRios.Common.Extensions;
using ArturRios.Common.Web.Api.Output;
using ArturRios.Common.Web.Security.Records;
using Newtonsoft.Json;

namespace ArturRios.Common.Web.Api.Client;

public abstract class BaseWebApiClientRoute(HttpClient httpClient)
{
    public abstract string BaseUrl { get; }

    protected async Task<Authentication> AuthenticateAsync(Credentials credentials, string authRoute)
    {
        var output = await PostAsync<Authentication>(authRoute, credentials);

        return output?.Data ?? throw new Exception("Could not authenticate");
    }

    protected void Authorize(string authToken) =>
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

    protected async Task AuthenticateAndAuthorizeAsync(Credentials credentials, string authRoute)
    {
        var authentication = await AuthenticateAsync(credentials, authRoute);

        Authorize(authentication.Token!);
    }

    protected async Task<WebApiOutput<To>?> GetAsync<To>(string route)
    {
        var response = await httpClient.GetAsync(route);

        return await DeserializeAsync<To>(response);
    }

    protected async Task<WebApiOutput<To>?> PatchAsync<To>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await httpClient.PatchAsync(route, payload);

        return await DeserializeAsync<To>(response);
    }

    protected async Task<WebApiOutput<To>?> PostAsync<To>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await httpClient.PostAsync(route, payload);

        return await DeserializeAsync<To>(response);
    }

    protected async Task<WebApiOutput<To>?> PutAsync<To>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await httpClient.PutAsync(route, payload);

        return await DeserializeAsync<To>(response);
    }

    protected async Task<WebApiOutput<To>?> DeleteAsync<To>(string route)
    {
        var response = await httpClient.DeleteAsync(route);

        return await DeserializeAsync<To>(response);
    }

    private static async Task<WebApiOutput<To>?> DeserializeAsync<To>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();

        var output = JsonConvert.DeserializeObject<WebApiOutput<To>>(body);

        return output;
    }
}
