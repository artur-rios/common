using ArturRios.Common.Pipelines.Commands.IO;

namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandPipeline
{
    Task<CommandOutput> ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand<CommandOutput>;
    Task<CommandOutput> ExecuteCommand(SerializedCommand commandInput);
}
