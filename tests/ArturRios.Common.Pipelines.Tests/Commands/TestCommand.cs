using ArturRios.Common.Pipelines.Commands;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class TestCommand : Command
{
    public string Message { get; set; } = string.Empty;
}
