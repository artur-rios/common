using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using ArturRios.Common.Aws.Lambda;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiResourceMethod : CfnMethod
{
    public AwsRestApiResourceMethod(HttpMethod method, AwsRestApiResource resource) : base(resource, method.ToString(),
        new CfnMethodProps { HttpMethod = method.ToString(), ResourceId = resource.Ref, RestApiId = resource.AwsRestApi.Ref })
    {
        resource.AwsRestApi.Stage.AddMethodSetting(new CfnStage.MethodSettingProperty
        {
            HttpMethod = method.ToString() == "ANY" ? "*" : method.ToString(), ResourcePath = resource.GetFullPath()
        });

        Resource = resource;
        AuthorizationType = "NONE";
    }

    public AwsRestApiResource Resource { get; }

    public void SetProxyIntegration(LambdaFunction lambdaFunction)
    {
        Integration = new IntegrationProperty
        {
            IntegrationHttpMethod = "POST",
            Type = "AWS_PROXY",
            Uri = Fn.Sub(
                "arn:aws:apigateway:${AWS::Region}:lambda:path/2015-03-31/functions/${ApiFunctionARN}/invocations",
                new Dictionary<string, string> { ["ApiFunctionARN"] = lambdaFunction.AttrArn })
        };

        lambdaFunction.AddPermission(Resource.AwsRestApi);

        AddDependency(lambdaFunction);
    }
}
