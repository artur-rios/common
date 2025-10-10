using ArturRios.Common.Data.Interfaces;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Tests.Commands;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Filters;
using ArturRios.Common.Pipelines.Tests.Output;

namespace ArturRios.Common.Pipelines.Tests.Handlers;

public class ScheduledTestCommandHandler(ICrudRepository<TestEntity> repository) : ICommandHandlerAsync<ScheduledTestCommand, ScheduledTestCommandOutput>
{
    public Task<ScheduledTestCommandOutput> HandleAsync(ScheduledTestCommand command)
    {
        var entity = repository.GetByFilter(new TestFilter { OperationId = command.OperationId }).FirstOrDefault() ?? throw new ArgumentException($"Entity with OperationId {command.OperationId} not found");

        entity.MarkAsCompleted();

        return Task.FromResult(new ScheduledTestCommandOutput { Messages = ["Command completed successfully"], Success = true, });
    }
}
