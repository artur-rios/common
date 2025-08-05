using ArturRios.Common.Pipelines.Commands.IO;

namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandler<in TCommand, TOutput>
    where TCommand : ICommand<TOutput>
    where TOutput : CommandOutput
{
    Task<TOutput> Handle(TCommand command);
}

public interface ICommandHandler<in TCommand, TInput, TOutput>
    where TCommand : ICommand<TInput, TOutput>
    where TInput : CommandInput
    where TOutput : CommandOutput
{
    Task<TOutput> Handle(TCommand command);
}
