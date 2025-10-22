using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Constructs;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApi : CfnRestApi
{
    private readonly List<AwsRestApiKey> _keys = [];
    private readonly List<AwsRestApiResource> _resources = [];

    public AwsRestApi(Construct scope, string constructId) : base(scope, constructId, new CfnRestApiProps())
    {
        ApiKeySourceType = "HEADER";
        EndpointConfiguration = new EndpointConfigurationProperty { Types = ["EDGE"] };
        Deployment = new AwsRestApiDeployment(this, Guid.NewGuid().ToString("N")[..4]);
        Stage = new AwsRestApiStage("ApiStage", Fn.Ref("Stage"), this);
        DefaultUsagePlan = new AwsRestApiUsagePlan(this, "DefaultUsagePlan", "DefaultUsagePlan", Stage);
    }

    public AwsRestApiStage Stage { get; }
    public AwsRestApiUsagePlan DefaultUsagePlan { get; }
    public AwsRestApiDeployment Deployment { get; }

    public AwsRestApi SetName(string name)
    {
        Name = name;

        return this;
    }

    public AwsRestApi SetDescription(string description)
    {
        Description = description;

        return this;
    }

    public AwsRestApiResource AddResource(string pathPart, AwsRestApiResource? parent = null)
    {
        var resource = parent is null
            ? new AwsRestApiResource(pathPart, this)
            : new AwsRestApiResource(pathPart, parent);

        _resources.Add(resource);

        return resource;
    }

    public AwsRestApiKey AddApiKey(string keyName, AwsRestApiUsagePlan? usagePlan = null)
    {
        var key = new AwsRestApiKey(this, keyName, keyName);

        _ = new AwsRestApiUsagePlanKey(this, $"UP{keyName}", key, usagePlan ?? DefaultUsagePlan);

        _keys.Add(key);

        return key;
    }
}
