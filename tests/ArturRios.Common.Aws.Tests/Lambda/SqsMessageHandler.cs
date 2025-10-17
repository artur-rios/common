using Amazon.Lambda.SQSEvents;
using ArturRios.Common.Aws.Sqs;
using ArturRios.Common.Extensions;
using ArturRios.Common.Output;
using ArturRios.Common.Pipelines;
using ArturRios.Common.Pipelines.Commands.Queues;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Aws.Tests.Lambda;

public class SqsMessageHandler(Pipeline pipeline, ILogger<SqsMessageHandler> logger) : ISqsMessageHandler
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

            var output = await pipeline.ExecuteCommandAsync<SerializedCommand, SerializedCommandOutput>(commandInput);

            var processOutput = new ProcessOutput();

            if (output.Success)
            {
                processOutput.AddMessage($"Command {commandInput.TypeFullName} | Id: {commandInput.CommandId} executed successfully");
            }

            logger.LogError("The command {CommandType} | Id: {CommandId} failed with errors: {Errors}",
                commandInput.TypeFullName, commandInput.CommandId, output.Errors.JoinWith());

            processOutput.AddErrors(output.Errors);

            return processOutput;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception raised from message body {MessageBody}", message.Body);

            var processOutput = new ProcessOutput();
            processOutput.AddError($"Exception raised from message body {message.Body}");
            processOutput.AddError($"Exception: {ex.Message}");

            return processOutput;
        }
    }
}
