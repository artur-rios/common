namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandler<in TCommand, out TOutput> where TCommand : Command where TOutput : CommandOutput
{
    TOutput Handle(TCommand command);
}
