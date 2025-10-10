namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandPipeline
{
    Task<CommandOutput> ExecuteCommandAsync<TCommand>(TCommand command) where TCommand : ICommand;
    Task<CommandOutput> ExecuteCommandAsync(SerializedCommand commandInput);
}
