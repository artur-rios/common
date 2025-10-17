using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandler<in TCommand, TOutput> where TCommand : Command where TOutput : CommandOutput
{
    DataOutput<TOutput?> Handle(TCommand command);
}
