using Amazon.CDK;

namespace ArturRios.Common.Aws;

public abstract class CloudFormationResourcesFactory
{
    public abstract App CreateDefaultApp();
}
