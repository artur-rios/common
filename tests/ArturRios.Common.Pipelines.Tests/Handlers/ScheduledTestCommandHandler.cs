using ArturRios.Common.Data.Interfaces;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Output;

namespace ArturRios.Common.Pipelines.Tests.Handlers;

public class ScheduledTestCommandHandler(ICrudRepository<TestEntity> repository) : ICommandHandlerAsync<ScheduledTestCommand, ScheduledTestCommandOutput>
{
    public Task<ScheduledTestCommandOutput> HandleAsync(ScheduledTestCommand command)
    {
        var entity = repository.GetById(command.Id) ?? throw new ArgumentException($"Entity with Id {command.Id} not found");

        entity.MarkAsCompleted();

        var output = new ScheduledTestCommandOutput();
        output.AddMessage("Command completed successfully");

        return Task.FromResult(output);
    }
}
