using Amazon.CDK.AWS.APIGateway;
using Constructs;

namespace ArturRios.Common.Aws.RestApi;

public class AwsRestApiUsagePlan(Construct scope, string id, string usagePlanName, AwsRestApiStage stage)
    : CfnUsagePlan(
        scope, id,
        new CfnUsagePlanProps
        {
            UsagePlanName = usagePlanName,
            ApiStages = new[] { new ApiStageProperty { ApiId = stage.AwsRestApi.Ref, Stage = stage.Ref } },
            Throttle = new ThrottleSettingsProperty { BurstLimit = 1000, RateLimit = 1000 }
        });
