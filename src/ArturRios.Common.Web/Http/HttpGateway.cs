using ArturRios.Common.Extensions;

namespace ArturRios.Common.Web.Http;

public class HttpGateway(HttpClient client)
{
    public HttpClient Client { get; } = client;

    public async Task<HttpOutput<TBody?>> GetAsync<TBody>(string route)
    {
        var response = await Client.GetAsync(route);

        return await ResolveResponseAsync<TBody?>(response);
    }

    public async Task<HttpOutput<TBody?>> PatchAsync<TBody>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PatchAsync(route, payload);

        return await ResolveResponseAsync<TBody?>(response);
    }

    public async Task<HttpOutput<TBody?>> PostAsync<TBody>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PostAsync(route, payload);

        return await ResolveResponseAsync<TBody?>(response);
    }

    public async Task<HttpOutput<TBody?>> PutAsync<TBody>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PutAsync(route, payload);

        return await ResolveResponseAsync<TBody?>(response);
    }

    public async Task<HttpOutput<TBody?>> DeleteAsync<TBody>(string route)
    {
        var response = await Client.DeleteAsync(route);

        return await ResolveResponseAsync<TBody?>(response);
    }

    private static async Task<HttpOutput<TBody?>> ResolveResponseAsync<TBody>(HttpResponseMessage response)
    {
        var output = new HttpOutput<TBody?>(response);
        await output.ReadContent();

        return output;
    }
}
