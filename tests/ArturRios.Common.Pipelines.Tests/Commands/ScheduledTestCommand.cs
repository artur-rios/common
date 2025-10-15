using ArturRios.Common.Pipelines.Commands;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduledTestCommand(Guid operationId) : Command
{
    public Guid OperationId { get; set; } = operationId;
}
