using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandlerAsync<in TCommand, TOutput> where TCommand : Command where TOutput : CommandOutput
{
    Task<DataOutput<TOutput>> HandleAsync(TCommand command);
}
