using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandler<in TCommand, out TOutput> where TCommand : Command where TOutput : ProcessOutput
{
    TOutput Handle(TCommand command);
}
