using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws;

public class RestApiDeployment(ApiGatewayRestApi api, string id)
    : CfnDeployment(api, id, new CfnDeploymentProps { RestApiId = api.Ref });
