namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandHandler<in TCommand, out TOutput>
    where TCommand : ICommand
    where TOutput : CommandOutput
{
    TOutput Handle(TCommand command);
}

public interface ICommandHandlerAsync<in TCommand, TOutput>
    where TCommand : ICommand
    where TOutput : CommandOutput
{
    Task<TOutput> HandleAsync(TCommand command);
}
