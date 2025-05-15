// ReSharper disable MemberCanBePrivate.Global
// Reason: This is a base test class, therefore the properties and methods should be accessible on derived classes

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This is a base test class, and the methods should be used on derived classes

using System.Net;
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
        var output = await Post<Authentication>(authRoute, credentials);

        return output.Data ?? throw new Exception("Could not authenticate");
    }
    
    public async Task<WebApiOutput<TO>> Get<TO>(string route, HttpStatusCode? expectedHttpStatusCode = null)
    {
        var response = await Client.GetAsync(route);

        TestHttpStatusCode(response.StatusCode, expectedHttpStatusCode);

        return await Deserialize<TO>(response);
    }
    
    public async Task<WebApiOutput<TO>> Patch<TO>(string route, object? payloadObject = null, HttpStatusCode? expectedHttpStatusCode = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PatchAsync(route, payload);
        
        TestHttpStatusCode(response.StatusCode, expectedHttpStatusCode);

        return await Deserialize<TO>(response);
    }

    public async Task<WebApiOutput<TO>> Post<TO>(string route, object? payloadObject = null, HttpStatusCode? expectedHttpStatusCode = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PostAsync(route, payload);
        
        TestHttpStatusCode(response.StatusCode, expectedHttpStatusCode);

        return await Deserialize<TO>(response);
    }
    
    public async Task<WebApiOutput<TO>> Put<TO>(string route, object? payloadObject = null, HttpStatusCode? expectedHttpStatusCode = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PutAsync(route, payload);
        
        TestHttpStatusCode(response.StatusCode, expectedHttpStatusCode);

        return await Deserialize<TO>(response);
    }
    
    public async Task<WebApiOutput<TO>> Delete<TO>(string route, HttpStatusCode? expectedHttpStatusCode = null)
    {
        var response = await Client.DeleteAsync(route);
        
        TestHttpStatusCode(response.StatusCode, expectedHttpStatusCode);

        return await Deserialize<TO>(response);
    }

    private static void TestHttpStatusCode(HttpStatusCode received, HttpStatusCode? expected)
    {
        if (expected is null)
        {
            return;
        }
        
        if (received != expected)
        {
            throw new TestException($"Expected HTTP status code {expected.ToString()} but received {received.ToString()}");
        }
    }
    
    private static async Task<WebApiOutput<TO>> Deserialize<TO>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        
        var output = JsonConvert.DeserializeObject<WebApiOutput<TO>>(body);

        return output ?? throw new TestException("Error when deserializing response body");
    }
}
