// ReSharper disable MemberCanBePrivate.Global
// Reason: This is a base test class, therefore the properties and methods should be accessible on derived classes

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This is a base test class, and the methods should be used on derived classes

// ReSharper disable InconsistentNaming
// Reason: these are not test methods, but utility methods for testing Web API endpoints

using System.Net;
using ArturRios.Common.Environment;
using ArturRios.Common.Extensions;
using ArturRios.Common.WebApi;
using ArturRios.Common.WebApi.Security.Records;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace ArturRios.Common.Test;

public class WebApiTest<T> where T : class
{
    private readonly WebApplicationFactory<T> _factory = new();
    public readonly HttpClient Client;

    public WebApiTest(EnvironmentType environment)
    {
        System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment.ToString().ToLower());

        Client = _factory.CreateClient();
    }

    public async Task<Authentication> Authenticate(Credentials credentials, string authRoute)
    {
        var output = await Post<Authentication>(authRoute, credentials);

        return output?.Data ?? throw new TestException("Could not authenticate");
    }

    public void Authorize(string authToken) => Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

    public async Task AuthenticateAndAuthorize(Credentials credentials, string authRoute)
    {
        var authentication = await Authenticate(credentials, authRoute);

        Authorize(authentication.Token!);
    }

    public async Task<WebApiOutput<TO>?> Get<TO>(string route, HttpStatusCode? expectedHttpStatusCode = null)
    {
        var response = await Client.GetAsync(route);

        return await TestHttpStatusCodeAndDeserialize<TO>(response, expectedHttpStatusCode);
    }

    public async Task<WebApiOutput<TO>?> Patch<TO>(string route, object? payloadObject = null,
        HttpStatusCode? expectedHttpStatusCode = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PatchAsync(route, payload);

        return await TestHttpStatusCodeAndDeserialize<TO>(response, expectedHttpStatusCode);
    }

    public async Task<WebApiOutput<TO>?> Post<TO>(string route, object? payloadObject = null,
        HttpStatusCode? expectedHttpStatusCode = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PostAsync(route, payload);

        return await TestHttpStatusCodeAndDeserialize<TO>(response, expectedHttpStatusCode);
    }

    public async Task<WebApiOutput<TO>?> Put<TO>(string route, object? payloadObject = null,
        HttpStatusCode? expectedHttpStatusCode = null)
    {
        var payload = payloadObject?.ToJsonStringContent();

        var response = await Client.PutAsync(route, payload);

        return await TestHttpStatusCodeAndDeserialize<TO>(response, expectedHttpStatusCode);
    }

    public async Task<WebApiOutput<TO>?> Delete<TO>(string route, HttpStatusCode? expectedHttpStatusCode = null)
    {
        var response = await Client.DeleteAsync(route);

        return await TestHttpStatusCodeAndDeserialize<TO>(response, expectedHttpStatusCode);
    }

    private static async Task<WebApiOutput<TO>?> TestHttpStatusCodeAndDeserialize<TO>(HttpResponseMessage response,
        HttpStatusCode? expectedHttpStatusCode = null)
    {
        var httpStatusCodeValid = TestHttpStatusCode(response.StatusCode, expectedHttpStatusCode);
        var result = await Deserialize<TO>(response);
        List<string> errors = [];

        if (!httpStatusCodeValid)
        {
            errors.Add(
                $"Expected HTTP status code {expectedHttpStatusCode.ToString()} but received {response.StatusCode.ToString()}");

            if (result is not null)
            {
                errors.AddRange(result.Messages);
            }
        }

        if (result is null)
        {
            errors.Add("Error when deserializing response body");
        }

        return errors.Count == 0 ? result : throw new TestException(string.Join("\n", errors));
    }

    private static bool TestHttpStatusCode(HttpStatusCode received, HttpStatusCode? expected = null)
    {
        if (expected is null)
        {
            return true;
        }

        return received == expected;
    }

    private static async Task<WebApiOutput<TO>?> Deserialize<TO>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();

        var output = JsonConvert.DeserializeObject<WebApiOutput<TO>>(body);

        return output;
    }
}
