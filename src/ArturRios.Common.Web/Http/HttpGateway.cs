using ArturRios.Common.Extensions;
using ArturRios.Common.Web.Api.Output;
using Newtonsoft.Json;

namespace ArturRios.Common.Web.Http;

public class HttpGateway(HttpClient client)
{
    public HttpClient Client { get; } = client;

    public async Task<WebApiOutput<TData?>> GetAsync<TData>(string route)
    {
        var response = await Client.GetAsync(route);

        return await DeserializeAsync<TData>(response);
    }

    public async Task<WebApiOutput<TData?>> PatchAsync<TData>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PatchAsync(route, payload);

        return await DeserializeAsync<TData>(response);
    }

    public async Task<WebApiOutput<TData?>> PostAsync<TData>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PostAsync(route, payload);

        return await DeserializeAsync<TData>(response);
    }

    public async Task<WebApiOutput<TData?>> PutAsync<TData>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PutAsync(route, payload);

        return await DeserializeAsync<TData>(response);
    }

    public async Task<WebApiOutput<TData?>> DeleteAsync<TData>(string route)
    {
        var response = await Client.DeleteAsync(route);

        return await DeserializeAsync<TData>(response);
    }

    private static async Task<WebApiOutput<TData?>> DeserializeAsync<TData>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();

        var output = JsonConvert.DeserializeObject<WebApiOutput<TData?>>(body);

        if (output is null)
        {
            return WebApiOutput<TData?>.New
                .WithData(default)
                .WithError("Failed to deserialize response body");
        }

        output.SetStatusCode(response.StatusCode);

        return output;
    }
}
