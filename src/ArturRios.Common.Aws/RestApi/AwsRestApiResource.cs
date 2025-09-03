using Amazon.CDK.AWS.APIGateway;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;


namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiResource : CfnResource
{
    // ReSharper disable CollectionNeverQueried.Local
    // Reason: needed for it's side effects
    private readonly List<AwsRestApiResourceMethod> _methods = [];

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

    // ReSharper disable once MemberCanBePrivate.Global
    // Reason: it might be useful for consumers of this class
    public AwsRestApiResource? Parent { get; }

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

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        // Reason: it ensures the loop continues traversing up the resource hierarchy until there are no more parent resources (i.e., resource becomes null)
        while (resource is not null)
        {
            parts.Add(resource.PathPart);
            resource = resource.Parent;
        }

        parts.Reverse();

        return string.Join("/", parts);
    }
}
