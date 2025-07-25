using Amazon.CDK;
using ArturRios.Common.Aws.Tests.WebApi;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;

namespace ArturRios.Common.Aws.Tests.Setup;

public class CloudFormationTest
{
    private const string StackName = "cf-test-stack";
    private CloudFormationStack? _stack = null;
    private SqsQueue? _testQueue = null;
    private LambdaFunction? _testLambda = null;

    public CloudFormationTest(App app)
    {
        var factory = new ResourcesFactory();

        _stack = factory.CreateDefaultStack(app, StackName);

        SetupWebApi(factory);
    }

    private void SetupWebApi(ResourcesFactory factory)
    {
        var apiLambdaHandler = factory.CreateDefaultLambda(_stack, "WebApiHandler")
            .SetFunctionName("test-web-api-handler")
            .SetTimeout(Duration.Seconds(30))
            .SetHandler<EntryPoint>(h => h.FunctionHandlerAsync)
            .SetCodeUri("../WebApi")
            .SetEnvironmentVariable("ENABLE_API_DOCS", Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "EnableApiDocs"));

        apiLambdaHandler.AddDependency(_testQueue);

        var api = new ApiGatewayRestApi(_stack, "WebApi")
            .SetName("test-web-api")
            .SetDescription("Test Web API");

        var apiResource = api.AddResource("api");
        var apiProxyResource = api.AddResource("{proxy+}", apiResource);
        var apiProxyMethod = apiProxyResource.AddMethod(HttpMethod.ANY);

        apiProxyMethod.SetProxyIntegration(apiLambdaHandler);
        apiProxyMethod.ApiKeyRequired = true;

        var swaggerResource = api.AddResource("swagger");
        var swaggerProxyResource = api.AddResource("{proxy+}", swaggerResource);
        var swaggerProxyMethod = swaggerProxyResource.AddMethod(HttpMethod.ANY);

        swaggerProxyMethod.SetProxyIntegration(apiLambdaHandler);
        swaggerProxyMethod.ApiKeyRequired = true;

        api.AddApiKey("Test-Api-Key");
    }
}
