using Amazon.Lambda.SQSEvents;
using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using ArturRios.Common.Pipelines;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Aws.Tests.Lambda;

public class LambdaHandler(ICommandPipeline pipeline, ILogger<LambdaHandler> logger) : ISqsMessageHandler
{
    public async Task<ProcessOutput> HandleAsync(SQSEvent.SQSMessage message)
    {
        try
        {
            var messageId = message.MessageId;

            logger.LogDebug("Begin processing message {MessageId}, with body {Body}", messageId, message.Body);

            var commandInput = SerializedCommand.FromJson(message.Body);

            logger.LogInformation("Executing command {CommandType} | Id: {CommandId}", commandInput.TypeFullName,
                commandInput.CommandId);

            var output = await pipeline.ExecuteCommand(commandInput);

            if (!output.Success)
            {
                logger.LogError("The command {CommandType} | Id: {CommandId} failed with errors: {Errors}",
                    commandInput.TypeFullName, commandInput.CommandId, output.Messages.JoinWith());

                return new ProcessOutput { Errors = output.Messages.ToList() };
            }

            return new ProcessOutput();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception raised from message body {MessageBody}", message.Body);

            return new ProcessOutput { Errors = [ex.Message] };
        }
    }
}
