using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using ArturRios.Common.Environment;
using Constructs;

namespace ArturRios.Common.Aws.Tests.Setup;

public class ResourcesFactory : CloudFormationResourcesFactory
{
    public string LambdaDefaultRoleArn = Fn.Sub("arn:aws:iam::${AWS::AccountId}:role/lambda-role");
    public Runtime LambdaDefaultRuntime = Runtime.DOTNET_8;
    public Duration LambdaDefaultTimeout = Duration.Seconds(60);
    public double LambdaDefaultMemorySize = 512;

    public List<string> DefaultStackTransforms = ["AWS:Serverless-2016-10-31"];

    public Dictionary<string, string> DefaultTags = new() { ["Project"] = "artur-rios-common-aws-tests" };

    private const string DefaultLambdaTimeZone = "America/Sao_Paulo";

    public Dictionary<string, string> DefaultLambdaEnvironmentVariables = new()
    {
        ["ASPNETCORE_ENVIRONMENT"] = Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "ENVIRONMENT"),
        ["TZ"] = DefaultLambdaTimeZone
    };

    public Dictionary<string, object> DefaultMappingValues = new()
    {
        ["test/sa-east-1/SubnetIds"] = new[] { "subnet-test1", "subnet-test2", "subnet-test3" },
        ["test/sa-east-1/SecurityGroupIds"] = new[] { "sg-test1" },
        ["test/sa-east-1/ENVIRONMENT"] = "Test",
        ["test/sa-east-1/RegionConfigName"] = "SA_EAST_1",
        ["test/sa-east-1/IntegrationEnvironment"] = "TEST"
    };

    public Dictionary<string, (string[] allowedValues, string defaultValue)> DefaultParameters = new()
    {
        ["Stage"] = (Enum.GetNames(typeof(EnvironmentType)), "Test")
    };

    public CloudFormationStack CreateDefaultStack(Construct scope, string stackName)
    {
        var stack = new CloudFormationStack(scope, stackName, new StackProps());

        foreach (var param in DefaultParameters)
        {
            stack.AddParameter(param.Key, param.Value.allowedValues, param.Value.defaultValue);
        }

        foreach (var mapping in DefaultMappingValues)
        {
            var keys = mapping.Key.Split('/');

            if (keys.Length != 3)
            {
                throw new ArgumentException(
                    $"The key '{mapping.Key}' must have the following format: '<map_name>/key1/key2'");
            }

            switch (mapping.Value)
            {
                case string value:
                    stack.AddMappingValue(keys[0], keys[1], keys[2], value);
                    break;
                case string[] values:
                    stack.AddMappingValues(keys[0], keys[1], keys[2], values);
                    break;
                default:
                    throw new ArgumentException(
                        $"The mapping value specified by key '{mapping.Key}' must be a string or an array of strings");
            }
        }

        foreach (var transform in DefaultStackTransforms)
        {
            stack.AddTransform(transform);
        }

        return stack;
    }

    public LambdaFunction CreateDefaultLambda(Construct scope, string id)
    {
        var lambdaFunction = new LambdaFunction(scope, id, LambdaDefaultRoleArn);
        lambdaFunction.SetRuntime(LambdaDefaultRuntime);
        lambdaFunction.SetTimeout(LambdaDefaultTimeout);

        foreach (var variable in DefaultLambdaEnvironmentVariables)
        {
            lambdaFunction.SetEnvironmentVariable(variable.Key, variable.Value);
        }

        lambdaFunction.SetVpcConfig(Token.AsList(Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "SubnetIds")),
            Token.AsList(Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "SecurityGroupIds")));

        lambdaFunction.MemorySize = LambdaDefaultMemorySize;

        return lambdaFunction;
    }

    public override App CreateDefaultApp()
    {
        return new App(new AppProps
        {
            DefaultStackSynthesizer = new DefaultStackSynthesizer(new DefaultStackSynthesizerProps
            {
                GenerateBootstrapVersionRule = false, Qualifier = null
            })
        });
    }
}
