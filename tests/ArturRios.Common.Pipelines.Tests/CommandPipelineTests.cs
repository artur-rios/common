using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Handlers;
using ArturRios.Common.Pipelines.Tests.Output;
using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Pipelines.Tests;

public class CommandPipelineTests
{
    [Fact]
    public async Task Should_ExecuteCommandAsync_And_Return_Output()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddTransient<ICommandHandlerAsync<TestCommand, TestCommandOutputAsync>, TestCommandHandlerAsync>()
            .BuildServiceProvider();

        var pipeline = new CommandPipeline(serviceProvider);
        var command = new TestCommand { Message = "{\"Id\":1,\"Name\":\"Test\"}" };

        var result = await pipeline.ExecuteAsync<TestCommandOutputAsync>(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Single(result.Messages);
        Assert.Equal("Message processed successfully", result.Messages[0]);
    }

    [Fact]
    public void Should_ExecuteCommand_And_Return_Output()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddTransient<ICommandHandler<TestCommand, TestCommandOutput>, TestCommandHandler>()
            .BuildServiceProvider();

        var pipeline = new CommandPipeline(serviceProvider);
        var command = new TestCommand { Message = "{\"Id\":1,\"Name\":\"Test\"}" };

        var result = pipeline.Execute<TestCommandOutput>(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Single(result.Messages);
        Assert.Equal("Message processed successfully", result.Messages[0]);
    }
}
