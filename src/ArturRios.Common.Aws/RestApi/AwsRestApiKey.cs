using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiKey(AwsRestApi awsRestApi, string id, string name) : CfnApiKey(awsRestApi, id,
    new CfnApiKeyProps
    {
        Enabled = true, Name = name, StageKeys = new[] { new StageKeyProperty { RestApiId = awsRestApi.Ref, StageName = awsRestApi.Stage.Ref } }
    });
