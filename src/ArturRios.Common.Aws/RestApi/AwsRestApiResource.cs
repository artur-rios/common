using Amazon.CDK.AWS.APIGateway;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiResource : CfnResource
{
    private readonly List<AwsRestApiResourceMethod> _methods = new();

    public AwsRestApiResource(string pathPart, AwsRestApi awsRestApi) : base(awsRestApi, pathPart,
        new CfnResourceProps { PathPart = pathPart, ParentId = awsRestApi.Ref, RestApiId = awsRestApi.AttrRootResourceId }) =>
        AwsRestApi = awsRestApi;

    public AwsRestApiResource(string pathPart, AwsRestApiResource parent) : base(parent, pathPart,
        new CfnResourceProps { PathPart = pathPart, ParentId = parent.Ref, RestApiId = parent.AwsRestApi.Ref })
    {
        AwsRestApi = parent.AwsRestApi;
        Parent = parent;
    }

    public AwsRestApi AwsRestApi { get; }
    public AwsRestApiResource Parent { get; }

    public AwsRestApiResourceMethod AddMethod(HttpMethod method)
    {
        var methodResource = new AwsRestApiResourceMethod(method, this);

        _methods.Add(methodResource);

        AwsRestApi.Deployment.AddDependency(methodResource);

        return methodResource;
    }

    public string GetFullPath()
    {
        var parts = new List<string>();
        var resource = this;

        while (resource != null)
        {
            parts.Add(resource.PathPart);
            resource = resource.Parent;
        }

        parts.Reverse();

        return string.Join("/", parts);
    }
}
