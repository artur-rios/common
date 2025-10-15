namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandlerAsync<in TCommand, TOutput> where TCommand : Command where TOutput : CommandOutput
{
    Task<TOutput> HandleAsync(TCommand command);
}
