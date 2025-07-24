using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws;

public class RestApiKey(ApiGatewayRestApi api, string id, string name) : CfnApiKey(api, id,
    new CfnApiKeyProps
    {
        Enabled = true, Name = name, StageKeys = new[] { new { RestApiId = api.Ref, StageName = api.Stage.Ref } }
    });
