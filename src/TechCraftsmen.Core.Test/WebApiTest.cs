// ReSharper disable MemberCanBePrivate.Global
// Reason: This is a base test class, therefore the properties and methods should be accessible on derived classes

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This is a base test class, and the methods should be used on derived classes

using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TechCraftsmen.Core.Environment;
using TechCraftsmen.Core.Extensions;
using TechCraftsmen.Core.WebApi;
using TechCraftsmen.Core.WebApi.Security.Records;

namespace TechCraftsmen.Core.Test;

public class WebApiTest<T> where T : class
{
    public readonly HttpClient Client;
    private readonly WebApplicationFactory<T> _factory = new();

    public WebApiTest(EnvironmentType environment)
    {
        System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment.ToString().ToLower());

        Client = _factory.CreateClient();
    }

    public async Task<Authentication> Authenticate(Credentials credentials, string authRoute)
    {
        var result = await Post<Authentication>(authRoute, credentials);

        return result.Data ?? throw new Exception("Could not authenticate");
    }
    
    public async Task<WebApiOutput<T>> Get(string route)
    {
        var response = await Client.GetAsync(route);

        var body = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<WebApiOutput<T>>(body);

        return result ?? throw new Exception("Error when deserializing Get request");
    }
    
    public async Task<WebApiOutput<T>> Patch(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PatchAsync(route, payload);

        var body = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<WebApiOutput<T>>(body);

        return result ?? throw new Exception("Error when deserializing Patch request");
    }

    public async Task<WebApiOutput<TO>> Post<TO>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PostAsync(route, payload);

        var body = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<WebApiOutput<TO>>(body);

        return result ?? throw new Exception("Error when deserializing Post request");
    }
    
    public async Task<WebApiOutput<TO>> Put<TO>(string route, object? payloadObject = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PutAsync(route, payload);

        var body = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<WebApiOutput<TO>>(body);

        return result ?? throw new Exception("Error when deserializing Put request");
    }
    
    public async Task<WebApiOutput<TO>> Delete<TO>(string route)
    {
        var response = await Client.DeleteAsync(route);

        var body = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<WebApiOutput<TO>>(body);

        return result ?? throw new Exception("Error when deserializing Delete request");
    }
}
