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
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddScoped<ICommandHandlerAsync<TestCommand, TestCommandOutputAsync>, TestCommandHandlerAsync>();

        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var pipeline = new Pipeline(provider.GetRequiredService<IServiceScopeFactory>());
        var command = new TestCommand { Message = "{\"Id\":1,\"Name\":\"Test\"}" };

        var result = await pipeline.ExecuteCommandAsync<TestCommand, TestCommandOutputAsync>(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Single(result.Messages);
        Assert.Equal("Message processed successfully", result.Messages[0]);
    }

    [Fact]
    public void Should_ExecuteCommand_And_Return_Output()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddScoped<ICommandHandler<TestCommand, TestCommandOutput>, TestCommandHandler>();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var pipeline = new Pipeline(provider.GetRequiredService<IServiceScopeFactory>());

        var command = new TestCommand { Message = "{\"Id\":1,\"Name\":\"Test\"}" };

        var result = pipeline.ExecuteCommand<TestCommand, TestCommandOutput>(command);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Single(result.Messages);
        Assert.Equal("Message processed successfully", result.Messages[0]);
    }
}
