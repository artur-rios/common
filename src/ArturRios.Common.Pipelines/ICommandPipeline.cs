using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines;

public interface ICommandPipeline
{
    Task<CommandOutput> ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand<CommandOutput>;
    Task<CommandOutput> ExecuteCommand(SerializedCommand commandInput);
}
