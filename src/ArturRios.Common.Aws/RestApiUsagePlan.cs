using Amazon.CDK.AWS.APIGateway;
using Constructs;

namespace ArturRios.Common.Aws;

public class RestApiUsagePlan(Construct scope, string id, string usagePlanName, RestApiStage stage) : CfnUsagePlan(
    scope, id,
    new CfnUsagePlanProps
    {
        UsagePlanName = usagePlanName,
        ApiStages = new[] { new ApiStageProperty { ApiId = stage.Api.Ref, Stage = stage.Ref } },
        Throttle = new ThrottleSettingsProperty { BurstLimit = 1000, RateLimit = 1000 }
    });
