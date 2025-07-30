using Amazon.CDK;

namespace ArturRios.Common.Aws;

public abstract class CloudFormationSetup
{
    public abstract void Init(App app);
}
