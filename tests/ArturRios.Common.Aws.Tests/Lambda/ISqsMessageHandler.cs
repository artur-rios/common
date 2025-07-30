using Amazon.Lambda.SQSEvents;
using ArturRios.Common.Output;

namespace ArturRios.Common.Aws.Tests.Lambda;

public interface ISqsMessageHandler
{
    Task<ProcessOutput> HandleAsync(SQSEvent.SQSMessage message);
}
