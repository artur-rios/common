using Amazon.CDK;
using ArturRios.Common.Aws.CloudFormation;
using ArturRios.Common.Aws.Lambda;
using ArturRios.Common.Aws.RestApi;
using ArturRios.Common.Aws.Sqs;
using ArturRios.Common.Aws.Tests.Lambda;
using ArturRios.Common.Aws.Tests.WebApi;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;

namespace ArturRios.Common.Aws.Tests.Setup;

public class TestCloudFormationSetup : CloudFormationSetup
{
    private const string StackName = "cf-test-stack";
    private CloudFormationStack? _stack;
    private SqsQueue? _testQueue;
    private LambdaFunction? _testLambda;

    public override void Init(App app)
    {
        var factory = new CloudFormationResourcesFactory();

        _stack = factory.CreateDefaultStack(app, StackName);

        SetupLambda(factory);
        SetupWebApi(factory);
    }

    private void SetupLambda(CloudFormationResourcesFactory factory)
    {
        _testQueue = new SqsQueue(_stack!, "TestQueue")
            .SetQueueName("test-queue")
            .SetVisibilityTimeout(Duration.Seconds(60));

        _testLambda = factory.CreateDefaultLambda(_stack!, "TestLambda")
            .SetFunctionName("test-lambda")
            .SetTimeout(Duration.Seconds(30))
            .SetHandler<LambdaTestEntryPoint>(h => h.Main)
            .SetCodeUri("../Lambda")
            .SetEnvironmentVariable("test-queue-arn", _testQueue.AttrArn)
            .SetEnvironmentVariable("test-queue-url", _testQueue.AttrQueueUrl);

        _testLambda.AddDependency(_testQueue);
        _testLambda.AddEventSource(_testQueue);
    }

    private void SetupWebApi(CloudFormationResourcesFactory factory)
    {
        var apiLambdaHandler = factory.CreateDefaultLambda(_stack!, "WebApiHandler")
            .SetFunctionName("test-web-api-handler")
            .SetTimeout(Duration.Seconds(30))
            .SetHandler<EntryPoint>(h => h.FunctionHandlerAsync)
            .SetCodeUri("../WebApi")
            .SetEnvironmentVariable("ENABLE_API_DOCS",
                Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "EnableApiDocs"));

        apiLambdaHandler.AddDependency(_testQueue!);

        var api = new AwsRestApi(_stack!, "WebApi")
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
