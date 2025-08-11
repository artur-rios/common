using ArturRios.Common.Data;
using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands.IO;
using ArturRios.Common.Pipelines.Tests.Entities;
using ArturRios.Common.Pipelines.Tests.Filters;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduledTestCommand(ICrudRepository<TestEntity> repository) : ICommandHandler<ScheduledTestCommand.Input, CommandOutput>
{
    public class Input(Guid operationId) : ICommand<CommandOutput>
    {
        public Guid OperationId { get; set; } = operationId;
    }

    public Task<CommandOutput> HandleAsync(Input command)
    {
        var entity = repository.GetByFilter(new TestFilter { OperationId = command.OperationId }).FirstOrDefault();

        if (entity is null)
        {
            throw new ArgumentException($"Entity with OperationId {command.OperationId} not found");
        }

        entity.MarkAsCompleted();

        return Task.FromResult(new CommandOutput { Messages = ["Command completed successfully"], Success = true, });
    }
}
