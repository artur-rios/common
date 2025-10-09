using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands.IO;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Util.Condition;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class TestCommand(ILogger<TestCommand> logger) : ICommandHandler<TestCommand.Input, CommandOutput>
{
    public class Input : ICommand<CommandOutput>
    {
        public string Message { get; set; } = string.Empty;
    }

    public Task<CommandOutput> HandleAsync(Input command)
    {
        Condition.Create
            .False(string.IsNullOrWhiteSpace(command.Message))
            .FailsWith("Message cannot be empty")
            .ThrowIfNotSatisfied();

        logger.LogInformation("Processing message: {CommandMessage}", command.Message);

        var entity = ParseMessage(command.Message);

        entity.DoSomething();

        return Task.FromResult(new CommandOutput { Messages = ["Message processed successfully"], Success = true });
    }

    private static TestEntity ParseMessage(string message)
    {
        return JsonConvert.DeserializeObject<TestEntity>(message)!;
    }
}
