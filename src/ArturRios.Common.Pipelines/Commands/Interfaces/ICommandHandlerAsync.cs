using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandlerAsync<in TCommand, TOutput> where TCommand : Command where TOutput : ProcessOutput
{
    Task<TOutput> HandleAsync(TCommand command);
}
