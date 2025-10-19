using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Web.Http;
using ArturRios.Common.Web.Security.Records;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ArturRios.Common.Test;

public class WebApiTest<T> where T : class
{
    private readonly WebApplicationFactory<T> _factory = new();
    protected readonly HttpGateway Gateway;

    protected WebApiTest(EnvironmentType environment)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment.ToString().ToLower());

        Gateway = new HttpGateway(_factory.CreateClient());
    }

    public async Task<Authentication> AuthenticateAsync(Credentials credentials, string authRoute)
    {
        var output = await Gateway.PostAsync<Authentication>(authRoute, credentials);

        return output.Body ?? throw new TestException("Could not authenticate");
    }

    public void Authorize(string authToken) =>
        Gateway.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

    public async Task AuthenticateAndAuthorizeAsync(Credentials credentials, string authRoute)
    {
        var authentication = await AuthenticateAsync(credentials, authRoute);

        Authorize(authentication.Token!);
    }
}
