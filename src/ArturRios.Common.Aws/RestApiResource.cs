// ReSharper disable InconsistentNaming
// Reason: this is not a test class

using Amazon.CDK.AWS.APIGateway;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;

namespace ArturRios.Common.Aws;

public class RestApiResource : CfnResource
{
    private readonly List<RestApiResourceMethod> _methods = new();

    public RestApiResource(string pathPart, ApiGatewayRestApi api) : base(api, pathPart,
        new CfnResourceProps { PathPart = pathPart, ParentId = api.Ref, RestApiId = api.AttrRootResourceId }) =>
        Api = api;

    public RestApiResource(string pathPart, RestApiResource parent) : base(parent, pathPart,
        new CfnResourceProps { PathPart = pathPart, ParentId = parent.Ref, RestApiId = parent.Api.Ref })
    {
        Api = parent.Api;
        Parent = parent;
    }

    public ApiGatewayRestApi Api { get; }
    public RestApiResource Parent { get; }

    public RestApiResourceMethod AddMethod(HttpMethod method)
    {
        var methodResource = new RestApiResourceMethod(method, this);

        _methods.Add(methodResource);

        Api.Deployment.AddDependency(methodResource);

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
