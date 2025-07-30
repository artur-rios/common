using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiUsagePlanKey(AwsRestApi awsRestApi, string id, AwsRestApiKey awsRestApiKey, AwsRestApiUsagePlan usagePlan)
    : CfnUsagePlanKey(awsRestApi, id,
        new CfnUsagePlanKeyProps { KeyId = awsRestApiKey.Ref, KeyType = "API_KEY", UsagePlanId = usagePlan.Ref });
