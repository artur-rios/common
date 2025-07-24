using Amazon.CDK.AWS.APIGateway;

namespace ArturRios.Common.Aws;

public class RestApiUsagePlanKey(ApiGatewayRestApi api, string id, RestApiKey restApiKey, RestApiUsagePlan usagePlan)
    : CfnUsagePlanKey(api, id, new CfnUsagePlanKeyProps
    {
        KeyId = restApiKey.Ref,
        KeyType = "API_KEY",
        UsagePlanId = usagePlan.Ref
    });
    