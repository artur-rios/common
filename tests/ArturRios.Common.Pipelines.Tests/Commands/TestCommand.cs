using ArturRios.Common.Pipelines.Commands.Interfaces;

namespace ArturRios.Common.Pipelines.Tests.Commands;

public class TestCommand : ICommand
{
    public string Message { get; set; } = string.Empty;
}
