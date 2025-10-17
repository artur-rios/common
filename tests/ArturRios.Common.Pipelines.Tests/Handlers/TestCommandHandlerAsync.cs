using ArturRios.Common.Output;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Output;
using ArturRios.Common.Util.Condition;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ArturRios.Common.Pipelines.Tests.Handlers;

public class TestCommandHandlerAsync(ILogger<TestCommand> logger) : ICommandHandlerAsync<TestCommand, TestCommandOutputAsync>
{
    public Task<DataOutput<TestCommandOutputAsync>> HandleAsync(TestCommand command)
    {
        Condition.Create
            .False(string.IsNullOrWhiteSpace(command.Message))
            .FailsWith("Message cannot be empty")
            .ThrowIfNotSatisfied();

        logger.LogInformation("Processing message: {CommandMessage}", command.Message);

        var entity = ParseMessage(command.Message);

        entity.DoSomething();

        var output = DataOutput<TestCommandOutputAsync>.New
            .WithData(new TestCommandOutputAsync())
            .WithMessage("Message processed successfully");

        return Task.FromResult(output);
    }

    private static TestEntity ParseMessage(string message)
    {
        return JsonConvert.DeserializeObject<TestEntity>(message)!;
    }
}
