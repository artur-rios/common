using Amazon.CDK;

namespace ArturRios.Common.Aws.CloudFormation;

public abstract class CloudFormationSetup
{
    public abstract void Init(App app);
}
