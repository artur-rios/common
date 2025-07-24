// ReSharper disable InconsistentNaming
// Reason: this is not a test class

using System.Collections.Concurrent;
using Amazon.CDK;
using Constructs;

namespace ArturRios.Common.Aws;

public class CloudFormationStack(Construct scope, string id, IStackProps props) : Stack(scope, id, props)
{
    private readonly ConcurrentDictionary<string, CfnCondition> _conditions = new();
    private readonly ConcurrentDictionary<string, CfnMapping> _mappings = new();
    private readonly ConcurrentDictionary<string, CfnParameter> _parameters = new();
    
    public CfnCondition AddCondition(string conditionKey, ICfnConditionExpression expression)
    {
        var condition = _conditions.GetOrAdd(conditionKey, key => new CfnCondition(this, key, new CfnConditionProps
        {
            Expression = expression
        }));
        
        condition.Expression = expression;

        return condition;
    }

    public void AddMappingValue(string mapName, string key1, string key2, string value)
    {
        var mapping = _mappings.GetOrAdd(mapName, _ => new CfnMapping(this, mapName, new CfnMappingProps
        {
            Mapping = new ConcurrentDictionary<string, IDictionary<string, object>>()
        }));
        
        mapping.SetValue(key1, key2, value);
    }
    
    public void AddMappingValues(string mapName, string key1, string key2, string[] values)
    {
        var mapping = _mappings.GetOrAdd(mapName, _ => new CfnMapping(this, mapName, new CfnMappingProps
        {
            Mapping = new ConcurrentDictionary<string, IDictionary<string, object>>()
        }));
        
        mapping.SetValue(key1, key2, values);
    }

    public void AddParameter(string parameterName, string[] allowedValues, string defaultValue)
    {
        var parameter = _parameters.GetOrAdd(parameterName,
            _ => new CfnParameter(this, parameterName, new CfnParameterProps()));
        
        parameter.AllowedValues = allowedValues;
        parameter.Default = defaultValue;
    }
}
