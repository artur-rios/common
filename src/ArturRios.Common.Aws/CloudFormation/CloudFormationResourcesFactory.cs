using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using ArturRios.Common.Aws.Lambda;
using ArturRios.Common.Configuration.Enums;
using Constructs;

namespace ArturRios.Common.Aws.CloudFormation;

public class CloudFormationResourcesFactory
{
    private const EnvironmentType DefaultEnvironment = EnvironmentType.Local;
    private const string RoleArnPrefix = "arn:aws:iam::${AWS::AccountId}:role/";
    private readonly string[] _defaultSecurityGroupIds = ["sg-test1"];


    private readonly List<string> _defaultStackTransforms = ["AWS:Serverless-2016-10-31"];
    private readonly string[] _defaultSubnetIds = ["subnet-test1", "subnet-test2", "subnet-test3"];

    private readonly Dictionary<string, string> _lambdaEnvironmentVariables = new()
    {
        ["ASPNETCORE_ENVIRONMENT"] = Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "ENVIRONMENT")
    };

    private readonly Dictionary<string, object> _stackMappingValues = new();

    private readonly Dictionary<string, (string[] allowedValues, string defaultValue)> _stackParameters = new()
    {
        ["Stage"] = (Enum.GetNames(typeof(EnvironmentType)), "Test")
    };

    private readonly Dictionary<string, string> _stackTags = new();

    private double _lambdaMemorySize = 512;
    private string _lambdaRegion = "sa-east-1";
    private string _lambdaRole = "test-role";
    private Runtime _lambdaRuntime = Runtime.DOTNET_8;
    private Duration _lambdaTimeout = Duration.Seconds(60);
    private string _lambdaTimeZone = "America/Sao_Paulo";

    private string _stackMappingName = "test";

    public static CloudFormationResourcesFactory New => new();

    public CloudFormationResourcesFactory SetEnvironment(EnvironmentType type)
    {
        _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/ENVIRONMENT", type.ToString());
        _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/IntegrationEnvironment",
            type.ToString().ToUpper());

        _stackParameters.Add("Stage", (Enum.GetNames(typeof(EnvironmentType)), nameof(EnvironmentType.Local)));

        return this;
    }

    public CloudFormationResourcesFactory SetLambdaMemorySize(double memorySize)
    {
        _lambdaMemorySize = memorySize;

        return this;
    }

    public CloudFormationResourcesFactory SetLambdaRole(string role)
    {
        _lambdaRole = role;

        return this;
    }

    public CloudFormationResourcesFactory SetLambdaRegion(string region)
    {
        _lambdaRegion = region;

        return this;
    }

    public CloudFormationResourcesFactory SetLambdaRuntime(Runtime runtime)
    {
        _lambdaRuntime = runtime;

        return this;
    }

    public CloudFormationResourcesFactory SetLambdaTimeout(Duration timeout)
    {
        _lambdaTimeout = timeout;

        return this;
    }

    public CloudFormationResourcesFactory SetLambdaTimeZone(string timeZone)
    {
        _lambdaTimeZone = timeZone;

        _lambdaEnvironmentVariables["TZ"] = timeZone;

        return this;
    }

    public CloudFormationResourcesFactory SetProjectName(string projectName)
    {
        _stackTags.Add("Project", projectName);

        return this;
    }

    public CloudFormationResourcesFactory SetStackMappingName(string mapName)
    {
        _stackMappingName = mapName;

        return this;
    }

    public CloudFormationResourcesFactory SetSecurityGroupIds(string[] securityGroupIds)
    {
        _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/SecurityGroupIds", securityGroupIds);

        return this;
    }

    public CloudFormationResourcesFactory SetSubnetIds(string[] subnetIds)
    {
        _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/SubnetIds", subnetIds);

        return this;
    }

    public CloudFormationStack CreateDefaultStack(Construct scope, string stackName)
    {
        var stack = new CloudFormationStack(scope, stackName, new StackProps());

        CheckEnvironmentVariables();
        CheckMappingValues();
        CheckParameters();
        CheckTags();

        foreach (var tag in _stackTags)
        {
            Tags.Of(stack).Add(tag.Key, tag.Value);
        }

        foreach (var param in _stackParameters)
        {
            stack.AddParameter(param.Key, param.Value.allowedValues, param.Value.defaultValue);
        }

        foreach (var mapping in _stackMappingValues)
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

        foreach (var transform in _defaultStackTransforms)
        {
            stack.AddTransform(transform);
        }

        return stack;
    }

    public LambdaFunction CreateDefaultLambda(Construct scope, string id)
    {
        var lambdaFunction = new LambdaFunction(scope, id, Fn.Sub($"{RoleArnPrefix}{_lambdaRole}"));
        lambdaFunction.SetRuntime(_lambdaRuntime);
        lambdaFunction.SetTimeout(_lambdaTimeout);

        foreach (var variable in _lambdaEnvironmentVariables)
        {
            lambdaFunction.SetEnvironmentVariable(variable.Key, variable.Value);
        }

        lambdaFunction.SetVpcConfig(Token.AsList(Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "SubnetIds")),
            Token.AsList(Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "SecurityGroupIds")));

        lambdaFunction.MemorySize = _lambdaMemorySize;

        return lambdaFunction;
    }

    private void CheckEnvironmentVariables()
    {
        var timeZone = _lambdaEnvironmentVariables.GetValueOrDefault("TZ");

        if (timeZone is null)
        {
            _lambdaEnvironmentVariables["TZ"] = _lambdaTimeZone;
        }
    }

    private void CheckMappingValues()
    {
        var subnetIds = _stackMappingValues.GetValueOrDefault($"{_stackMappingName}/{_lambdaRegion}/SubnetIds");

        if (subnetIds is null)
        {
            _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/SubnetIds", _defaultSubnetIds);
        }

        var securityGroupIds =
            _stackMappingValues.GetValueOrDefault($"{_stackMappingName}/{_lambdaRegion}/SecurityGroupIds");

        if (securityGroupIds is null)
        {
            _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/SecurityGroupIds", _defaultSecurityGroupIds);
        }

        var environment = _stackMappingValues.GetValueOrDefault($"{_stackMappingName}/{_lambdaRegion}/ENVIRONMENT");

        if (environment is null)
        {
            _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/ENVIRONMENT", DefaultEnvironment.ToString());
        }

        var integrationEnvironment =
            _stackMappingValues.GetValueOrDefault($"{_stackMappingName}/{_lambdaRegion}/IntegrationEnvironment");

        if (integrationEnvironment is null)
        {
            _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/IntegrationEnvironment",
                DefaultEnvironment.ToString().ToUpper());
        }

        var regionConfigName =
            _stackMappingValues.GetValueOrDefault($"{_stackMappingName}/{_lambdaRegion}/RegionConfigName");

        if (regionConfigName is null)
        {
            _stackMappingValues.Add($"{_stackMappingName}/{_lambdaRegion}/RegionConfigName",
                _lambdaRegion.ToUpper().Replace("-", "_"));
        }
    }

    private void CheckParameters()
    {
        if (!_stackParameters.ContainsKey("Stage"))
        {
            _stackParameters.Add("Stage", (Enum.GetNames(typeof(EnvironmentType)), nameof(EnvironmentType.Local)));
        }
    }

    private void CheckTags()
    {
        var projectName = _stackTags.GetValueOrDefault("Project");

        if (projectName is null)
        {
            _stackTags.Add("Project", "artur-rios-common-aws-tests");
        }
    }
}
