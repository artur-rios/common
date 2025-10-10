using Amazon.Lambda.SQSEvents;
using ArturRios.Common.Aws.Sqs;
using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using ArturRios.Common.Pipelines.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Pipelines.Tests.Lambda;

public class IncomingMessagesHandler(IPipeline pipeline, ILogger<IncomingMessagesHandler> logger) : ISqsMessageHandler
{
    public async Task<ProcessOutput> HandleAsync(SQSEvent.SQSMessage message)
    {
        try
        {
            logger.LogDebug("Processing incoming SQS message with Id {MessageId} and Body {MessageBody}",
                message.MessageId, message.Body);

            var command = new TestCommand { Message = message.Body };

            logger.LogInformation("Executing command {CommandType} with Data: {CommandData}", command.GetType().Name,
                command);

            var result = await pipeline.ExecuteCommandAsync(command);

            if (!result.Success)
            {
                logger.LogError("Command execution failed with errors: {ErrorMessages}", result.Messages.JoinWith());

                return new ProcessOutput { Errors = result.Messages };
            }

            logger.LogInformation("Command executed successfully");

            return new ProcessOutput();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing SQS message");

            return new ProcessOutput { Errors = [ex.Message] };
        }
    }
}
