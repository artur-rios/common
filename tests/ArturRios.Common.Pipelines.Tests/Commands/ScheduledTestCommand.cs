using ArturRios.Common.Pipelines.Commands;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class ScheduledTestCommand(int id) : Command
{
    public int Id { get; set; } = id;
}
