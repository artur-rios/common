using Amazon.Lambda.SQSEvents;
using ArturRios.Common.Output;

namespace ArturRios.Common.Aws.Sqs;

public interface ISqsMessageHandler
{
    Task<ProcessOutput> HandleAsync(SQSEvent.SQSMessage message);
}
