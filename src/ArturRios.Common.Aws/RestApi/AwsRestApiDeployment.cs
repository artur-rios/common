using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiDeployment(AwsRestApi awsRestApi, string id)
    : CfnDeployment(awsRestApi, id, new CfnDeploymentProps { RestApiId = awsRestApi.Ref });
