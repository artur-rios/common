// ReSharper disable InconsistentNaming
// Reason: this is not a test class

using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SQS;
using Constructs;

namespace ArturRios.Common.Aws;

public class SqsQueue : CfnQueue
{
    public SqsQueue(Construct scope, string id) : base(scope, id, new CfnQueueProps()) => Tags.SetDefaultTags();

    public SqsQueue SetQueueName(string queueName)
    {
        QueueName = queueName;

        return this;
    }

    public SqsQueue EnableFifo()
    {
        FifoQueue = true;

        if (!QueueName!.EndsWith(".fifo"))
        {
            QueueName = $"{QueueName}.fifo";
        }

        return this;
    }

    public SqsQueue DisableFifo()
    {
        FifoQueue = false;

        if (QueueName!.EndsWith(".fifo"))
        {
            QueueName = QueueName[..^".fifo".Length];
        }

        return this;
    }

    public SqsQueue SetVisibilityTimeout(Duration duration)
    {
        VisibilityTimeout = duration.ToSeconds();

        return this;
    }

    public SnsSubscription SubscribeToTopic(string topicArn, string topicName, string region)
    {
        var subscription = new SnsSubscription(this, $"subscription-{topicName}",
            new CfnSubscriptionProps
            {
                Endpoint = AttrArn,
                Protocol = "sqs",
                RawMessageDelivery = true,
                Region = region,
                TopicArn = topicArn
            });

        // ReSharper disable once UnusedVariable
        // Reason: this variable is used for its side effects
        var policy = new CfnQueuePolicy(this, $"policy-{topicName}", new CfnQueuePolicyProps
        {
            Queues = [AttrQueueUrl],
            PolicyDocument = new PolicyDocument(new PolicyDocumentProps
            {
                Statements =
                [
                    new PolicyStatement(new PolicyStatementProps
                    {
                        Effect = Effect.ALLOW,
                        Actions = ["sqs:SendMessage"],
                        Resources = [AttrArn],
                        Principals = [new ServicePrincipal("sns.amazonaws.com")],
                        Conditions = new Dictionary<string, object>
                        {
                            ["ArnEquals"] = new Dictionary<string, object> { ["aws:SourceArn"] = topicArn }
                        }
                    })
                ]
            })
        });

        return subscription;
    }
}
