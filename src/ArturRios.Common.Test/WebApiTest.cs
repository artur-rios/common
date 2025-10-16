// ReSharper disable MemberCanBePrivate.Global
// Reason: This is a base test class, therefore the properties and methods should be accessible on derived classes

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This is a base test class, and the methods should be used on derived classes

using System.Net;
using ArturRios.Common.Configuration.Enums;
using ArturRios.Common.Extensions;
using ArturRios.Common.Web;
using ArturRios.Common.Web.Api.Output;
using ArturRios.Common.Web.Http;
using ArturRios.Common.Web.Security.Records;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

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

        return output.Data ?? throw new TestException("Could not authenticate");
    }

    public void Authorize(string authToken) =>
        Gateway.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

    public async Task AuthenticateAndAuthorizeAsync(Credentials credentials, string authRoute)
    {
        var authentication = await AuthenticateAsync(credentials, authRoute);

        Authorize(authentication.Token!);
    }
}
