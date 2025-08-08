using System.Net;
using Amazon.Scheduler;
using Amazon.Scheduler.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using ArturRios.Common.Extensions;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Pipelines.Commands.Queues;

public class AwsCommandQueue(
    IAmazonSQS sqs,
    ILogger<AwsCommandQueue> logger,
    IAmazonScheduler scheduler,
    AwsCommandQueueConfiguration configuration) : ICommandQueue
{
    private readonly List<SerializedCommand> commandsQueue = [];

    public void Enqueue(object command)
    {
        commandsQueue.Add(SerializedCommand.FromRequest(command));
    }

    public async Task Flush()
    {
        var enqueueTask = FlushEnqueuedCommands(commandsQueue.FindAll(command => !command.IsScheduled));
        var scheduleTask =
            FlushScheduledCommands(new Queue<SerializedCommand>(commandsQueue.Where(command => command.IsScheduled)));

        await enqueueTask;
        await scheduleTask;
    }

    private async Task FlushEnqueuedCommands(List<SerializedCommand> enqueuedCommands)
    {
        const int maxFails = 5;
        var fails = 0;

        logger.LogDebug("Flushing a total of {Count} enqueued commands", enqueuedCommands.Count);

        while (enqueuedCommands.Count > 0)
        {
            var chuckSize = enqueuedCommands.Count >= 10 ? 10 : enqueuedCommands.Count;
            var chunk = enqueuedCommands.GetRange(0, chuckSize);

            enqueuedCommands.RemoveRange(0, chuckSize);

            logger.LogDebug("Flushing a chunk of {Count} commands", chuckSize);

            foreach (var command in chunk)
            {
                logger.LogDebug("Flushing command {Command}", command.TypeFullName);
            }

            var response = await sqs.SendMessageBatchAsync(new SendMessageBatchRequest
            {
                QueueUrl = configuration.QueueUrl,
                Entries = chunk.Select((c, i) => new SendMessageBatchRequestEntry
                {
                    Id = i.ToString(),
                    MessageBody = c.ToJson(),
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        ["CommandType"] = new() { StringValue = c.TypeFullName, DataType = "String" }
                    }
                }).ToList()
            });

            logger.LogDebug(
                "Commands sent: Status - {StatusCode} | SuccessCount - {SuccessCount} | FailedCount - {FailedCount}",
                response.HttpStatusCode, response.Successful?.Count ?? 0, response.Failed?.Count ?? 0);

            if (response.Failed.IsEmpty())
            {
                continue;
            }

            if (++fails > maxFails)
            {
                throw new InvalidOperationException($"Even after {maxFails} tries it was not possible to publish all the commands. Failed count: {response.Failed!.Count}");
            }

            var orderedFailed = response.Failed!.Select(f => int.Parse(f.Id)).OrderByDescending(f => f);

            enqueuedCommands.AddRange(orderedFailed.Select(failed => chunk[failed]));
        }
    }

    private async Task FlushScheduledCommands(Queue<SerializedCommand> scheduledCommands)
    {
        const int maxFails = 5;
        var fails = 0;

        logger.LogDebug("Flushing a total of {Count} scheduled commands", scheduledCommands.Count);

        while (scheduledCommands.Count > 0)
        {
            var command = scheduledCommands.Dequeue();

            var payload = new CreateScheduleRequest
            {
                Name = $"command-queue-{command.CommandId}",
                ScheduleExpression = $"at({command.ScheduledAt!.Value:yyyy-MM-ddTHH:mm:ss})",
                Target = new Target
                {
                    Arn = configuration.QueueArn,
                    RetryPolicy = new RetryPolicy { MaximumRetryAttempts = 5 },
                    Input = command.ToJson(),
                    RoleArn = configuration.SchedulerRoleArn
                },
                Description = $"Command queue schedule for {command.TypeFullName} with ID {command.CommandId}",
                State = ScheduleState.ENABLED,
                ActionAfterCompletion = ActionAfterCompletion.DELETE,
                FlexibleTimeWindow = new FlexibleTimeWindow
                {
                    Mode = FlexibleTimeWindowMode.OFF
                }
            };

            logger.LogInformation("Creating schedule: {Payload}", command.ToJson());

            var response = await scheduler.CreateScheduleAsync(payload);

            if (response.HttpStatusCode is not HttpStatusCode.OK)
            {
                if (++fails > maxFails)
                {
                    throw new InvalidOperationException($"Even after {maxFails} tries it was not possible to schedule the command {command.TypeFullName} with ID {command.CommandId}");
                }

                logger.LogWarning("Failed to create schedule for command {CommandId}. Retrying...", command.CommandId);

                scheduledCommands.Enqueue(command);
            }
            else
            {
                logger.LogInformation("Successfully created schedule with Arn {ScheduleArn} for command {CommandId}", response.ScheduleArn, command.CommandId);
            }
        }
    }

    public void Schedule(object command, DateTime dueDate)
    {
        commandsQueue.Add(SerializedCommand.FromScheduledRequest(command, dueDate));
    }
}
