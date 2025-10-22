using ArturRios.Common.Output;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Output;
using ArturRios.Common.Util.Condition;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ArturRios.Common.Pipelines.Tests.Handlers;

public class TestCommandHandler(ILogger<TestCommand> logger) : ICommandHandler<TestCommand, TestCommandOutput>
{
    public DataOutput<TestCommandOutput?> Handle(TestCommand command)
    {
        Condition.Create
            .False(string.IsNullOrWhiteSpace(command.Message))
            .FailsWith("Message cannot be empty")
            .ThrowIfNotSatisfied();

        logger.LogInformation("Processing message: {CommandMessage}", command.Message);

        var entity = ParseMessage(command.Message);

        entity.DoSomething();

        var output = DataOutput<TestCommandOutput?>.New
            .WithData(new TestCommandOutput())
            .WithMessage("Message processed successfully");

        return output;
    }

    private static TestEntity ParseMessage(string message) => JsonConvert.DeserializeObject<TestEntity>(message)!;
}
