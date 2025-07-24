using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.SNS;
using Constructs;

namespace ArturRios.Common.Aws;

public class SnsTopic : CfnTopic
{
    CfnTopicPolicy? _topicPolicy;
    List<PolicyStatement>? _policyStatements;
    
    public SnsTopic(Construct scope, string id) : base(scope, id, new CfnTopicProps())
    {
        Tags.SetDefaultTags();
    }

    public SnsTopic SetTopicName(string name)
    {
        TopicName = name;
        
        return this;
    }

    public SnsTopic AllowAwsAccount(string awsAccountId)
    {
        _policyStatements ??= [];

        _topicPolicy ??= new CfnTopicPolicy(this, $"Policy{TopicName}", new CfnTopicPolicyProps
        {
            Topics = [AttrTopicArn],
            PolicyDocument = new PolicyDocument(new PolicyDocumentProps
            {
                Statements = _policyStatements.ToArray()
            })
        });
        
        _policyStatements.Add(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = ["sns:Subscribe", "sns:Receive"],
            Resources = [AttrTopicArn],
            Principals = [new AccountPrincipal(awsAccountId)]
        }));

        _topicPolicy.PolicyDocument = new PolicyDocument(new PolicyDocumentProps
        {
            Statements = _policyStatements.ToArray()
        });
        
        return this;
    }
}
