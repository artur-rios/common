
using ArturRios.Common.Pipelines.Commands.Interfaces;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduledTestCommand(Guid operationId) : ICommand
{
    public Guid OperationId { get; set; } = operationId;
}
