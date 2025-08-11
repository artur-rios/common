using Amazon.Lambda.SQSEvents;
using ArturRios.Common.Pipelines.Messaging;

namespace ArturRios.Common.Aws.Sqs;

public interface ISqsMessageHandler : IMessageHandler<SQSEvent.SQSMessage>;
