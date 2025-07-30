using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiStage(string id, string stageName, AwsRestApi awsRestApi) : CfnStage(awsRestApi, id,
    new CfnStageProps { RestApiId = awsRestApi.Ref, StageName = stageName, DeploymentId = awsRestApi.Deployment.Ref })
{
    private readonly List<MethodSettingProperty> _methodSettings = [];

    public AwsRestApi AwsRestApi { get; } = awsRestApi;

    public void AddMethodSetting(MethodSettingProperty methodSettingProperty)
    {
        _methodSettings.Add(methodSettingProperty);
        MethodSettings = _methodSettings.ToArray();
    }
}
