// ReSharper disable VirtualMemberCallInConstructor
// Reason: necessary calls

using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Constructs;

namespace ArturRios.Common.Aws;

public class ApiGatewayRestApi : CfnRestApi
{
    // ReSharper disable once CollectionNeverQueried.Local
    // Reason: necessary for it's side effects
    private readonly List<RestApiKey> _keys = [];

    // ReSharper disable once CollectionNeverQueried.Local
    // Reason: necessary for it's side effects
    private readonly List<RestApiResource> _resources = [];

    public ApiGatewayRestApi(Construct scope, string constructId) : base(scope, constructId, new CfnRestApiProps())
    {
        ApiKeySourceType = "HEADER";
        EndpointConfiguration = new EndpointConfigurationProperty { Types = ["EDGE"] };
        Deployment = new RestApiDeployment(this, Guid.NewGuid().ToString("N")[..4]);
        Stage = new RestApiStage("ApiStage", Fn.Ref("Stage"), this);
        DefaultUsagePlan = new RestApiUsagePlan(this, "DefaultUsagePlan", "DefaultUsagePlan", Stage);
    }

    public RestApiStage Stage { get; }
    public RestApiUsagePlan DefaultUsagePlan { get; }
    public RestApiDeployment Deployment { get; }

    public ApiGatewayRestApi SetName(string name)
    {
        Name = name;

        return this;
    }

    public ApiGatewayRestApi SetDescription(string description)
    {
        Description = description;

        return this;
    }

    public RestApiResource AddResource(string pathPart, RestApiResource? parent = null)
    {
        var resource = parent is null ? new RestApiResource(pathPart, this) : new RestApiResource(pathPart, parent);

        _resources.Add(resource);

        return resource;
    }

    public RestApiKey AddApiKey(string keyName, RestApiUsagePlan? usagePlan = null)
    {
        var key = new RestApiKey(this, keyName, keyName);

        _ = new RestApiUsagePlanKey(this, $"UP{keyName}", key, usagePlan ?? DefaultUsagePlan);

        _keys.Add(key);

        return key;
    }
}
