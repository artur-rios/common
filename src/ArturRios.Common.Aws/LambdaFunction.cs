using System.Linq.Expressions;
using System.Reflection;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace ArturRios.Common.Aws;

public class LambdaFunction : CfnFunction
{
    private readonly Dictionary<string, string> _environmentVariables = new();
    private readonly List<CfnEventSourceMapping> _eventSources;
    private readonly Dictionary<string, CfnPermission> _permissions = new();

    public LambdaFunction(Construct scope, string constructId, string roleArn) : base(scope, constructId,
        new CfnFunctionProps { Code = new CodeProperty(), Role = roleArn })
    {
        _eventSources = [];

        SetEnvironmentVariable($"REFRESH_STACK_TOKEN{Guid.NewGuid():N}", string.Empty);
    }

    public LambdaFunction SetFunctionName(string name)
    {
        FunctionName = name;

        return this;
    }

    public LambdaFunction SetTimeout(Duration duration)
    {
        Timeout = duration.ToSeconds();

        return this;
    }

    public LambdaFunction SetRuntime(Runtime runtime)
    {
        Runtime = runtime.Name;

        return this;
    }

    public LambdaFunction SetHandler<T>(Expression<Func<T, Delegate>> expression)
    {
        var exp = expression.Body.NodeType == ExpressionType.Convert
            ? (expression.Body as UnaryExpression)?.Operand
            : expression.Body;

        var createDelegate = (exp as MethodCallExpression)?.Object;
        var methodInfo = (createDelegate as ConstantExpression)?.Value as MethodInfo;
        var @class = typeof(T);
        var assembly = @class.Assembly;

        Handler = $"{assembly.GetName().Name}::{@class.FullName}::{methodInfo?.Name}";

        return this;
    }

    public LambdaFunction SetHandler(string name)
    {
        Handler = name;

        return this;
    }

    public LambdaFunction SetEnvironmentVariable(string key, string value)
    {
        _environmentVariables[key] = value;

        Environment = new EnvironmentProperty { Variables = _environmentVariables };

        return this;
    }

    public LambdaFunction SetRole(string roleArn)
    {
        Role = roleArn;

        return this;
    }

    public LambdaFunction SetVpcConfig(string[] subnetIds, string[] securityGroupIds)
    {
        VpcConfig = new VpcConfigProperty { SubnetIds = subnetIds, SecurityGroupIds = securityGroupIds };

        return this;
    }

    public LambdaFunction SetCodeUri(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            throw new ArgumentException("Code URI cannot be null or empty.", nameof(uri));
        }

        AddPropertyOverride("CodeUri", uri);
        AddPropertyDeletionOverride("Code");
        AddOverride("Type", "AWS::Serverless::Function");

        return this;
    }

    public CfnEventSourceMapping AddEventSource(SqsQueue sqsQueue, int batchSize = 10,
        bool reportBatchItemFailures = true)
    {
        var eventSourceMappingProps = new CfnEventSourceMappingProps
        {
            EventSourceArn = sqsQueue.AttrArn, FunctionName = FunctionName!, Enabled = true, BatchSize = batchSize
        };

        if (reportBatchItemFailures)
        {
            eventSourceMappingProps.FunctionResponseTypes = ["ReportBatchItemFailures"];
        }

        var sourceId = Stack.GetLogicalId(sqsQueue);
        var eventSourceMapping = new CfnEventSourceMapping(this, $"TR{sourceId}", eventSourceMappingProps);

        _eventSources.Add(eventSourceMapping);

        eventSourceMapping.AddDependency(sqsQueue);
        eventSourceMapping.AddDependency(this);

        return eventSourceMapping;
    }

    public CfnPermission AddPermission(ApiGatewayRestApi apiGatewayRestApi)
    {
        var key = $"permission-{Stack.GetLogicalId(apiGatewayRestApi)}";

        if (_permissions.TryGetValue(key, out var existingPermission))
        {
            return existingPermission;
        }

        var permission = new CfnPermission(this, key, new CfnPermissionProps
        {
            Action = "lambda:InvokeFunction",
            FunctionName = FunctionName!,
            Principal = "apigateway.amazonaws.com",
            SourceArn = Fn.Sub(
                $"arn:aws:execute-api:${{AWS::Region}}:${{AWS::AccountId}}:${{{apiGatewayRestApi.LogicalId}}}/*/*")
        });

        _permissions[key] = permission;

        permission.AddDependency(this);

        return permission;
    }
}
