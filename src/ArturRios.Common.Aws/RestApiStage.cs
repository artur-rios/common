// ReSharper disable InconsistentNaming
// Reason: this is not a test class

using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws;

public class RestApiStage(string id, string stageName, ApiGatewayRestApi api) : CfnStage(api, id, new CfnStageProps
{
    RestApiId = api.Ref,
    StageName = stageName,
    DeploymentId = api.Deployment.Ref
})
{
    private readonly List<MethodSettingProperty> _methodSettings = [];
    
    public ApiGatewayRestApi Api { get; } = api;

    public void AddMethodSetting(MethodSettingProperty methodSettingProperty)
    {
        _methodSettings.Add(methodSettingProperty);
        MethodSettings = _methodSettings.ToArray();
    }
}
