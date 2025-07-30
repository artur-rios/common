using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;

namespace ArturRios.Common.Aws;

public class RestApiResourceMethod : CfnMethod
{
    public RestApiResourceMethod(HttpMethod method, RestApiResource resource) : base(resource, method.ToString(),
        new CfnMethodProps { HttpMethod = method.ToString(), ResourceId = resource.Ref, RestApiId = resource.Api.Ref })
    {
        resource.Api.Stage.AddMethodSetting(new CfnStage.MethodSettingProperty
        {
            HttpMethod = method.ToString() == "ANY" ? "*" : method.ToString(), ResourcePath = resource.GetFullPath()
        });

        Resource = resource;
        AuthorizationType = "NONE";
    }

    public RestApiResource Resource { get; }

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

        lambdaFunction.AddPermission(Resource.Api);

        AddDependency(lambdaFunction);
    }
}
