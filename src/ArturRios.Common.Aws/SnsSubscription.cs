using Amazon.CDK.AWS.SNS;
using Constructs;

namespace ArturRios.Common.Aws;

public class SnsSubscription(Construct scope, string id, ICfnSubscriptionProps props) : CfnSubscription(scope, id, props);
