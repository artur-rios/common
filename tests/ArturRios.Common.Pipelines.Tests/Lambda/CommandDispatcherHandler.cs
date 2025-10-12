using Amazon.Lambda.SQSEvents;
using ArturRios.Common.Aws.Sqs;
using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using ArturRios.Common.Pipelines.Commands;
using ArturRios.Common.Pipelines.Interfaces;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Pipelines.Tests.Lambda;

public class CommandDispatcherHandler(IPipeline pipeline, ILogger<CommandDispatcherHandler> logger) : ISqsMessageHandler
{
    public async Task<ProcessOutput> HandleAsync(SQSEvent.SQSMessage message)
    {
        try
        {
            logger.LogDebug("Begin processing SQS message with Id {MessageId} and Body {MessageBody}",
                message.MessageId, message.Body);

            var commandInput = SerializedCommand.FromJson(message.Body);

            logger.LogInformation("Executing command {CommandName} with Id {CommandId} and Data: {CommandData}",
                commandInput.TypeFullName, commandInput.CommandId, commandInput.Data);

            var commandOutput = await pipeline.ExecuteCommandAsync(commandInput);

            if (!commandOutput.Success)
            {
                logger.LogError(
                    "The command {CommandType} with Id {commandId} has failed to execute. Errors: {ErrorMessages}",
                    commandInput.TypeFullName, commandInput.CommandId, commandOutput.Messages.JoinWith());
            }

            var output = new ProcessOutput();
            output.AddMessages(commandOutput.Messages);

            if (!commandOutput.Success)
            {
                output.AddErrors(commandOutput.Errors);
            }

            return output;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Exception thrown when processing SQS message with Id {MessageId} and Body {MessageBody}",
                message.MessageId, message.Body);

            var output = new ProcessOutput();
            output.AddError(ex.Message);

            return output;
        }
    }
}
