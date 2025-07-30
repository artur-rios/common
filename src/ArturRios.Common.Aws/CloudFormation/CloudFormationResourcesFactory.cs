using Amazon.CDK;

namespace ArturRios.Common.Aws.CloudFormation;

public abstract class CloudFormationResourcesFactory
{
    public abstract App CreateDefaultApp();
}
