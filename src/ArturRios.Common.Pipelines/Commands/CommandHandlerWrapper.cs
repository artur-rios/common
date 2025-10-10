using ArturRios.Common.Pipelines.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Pipelines.Commands;

internal abstract class CommandHandlerWrapper
{
    public abstract Task<object> HandleAsync(object command, IServiceProvider serviceProvider);
    public abstract object Handle(object command, IServiceProvider serviceProvider);
}

internal class CommandHandlerWrapper<TCommand, TOutput> : CommandHandlerWrapper
    where TCommand : ICommand
    where TOutput : CommandOutput
{
    public override async Task<object> HandleAsync(object command, IServiceProvider serviceProvider)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandlerAsync<TCommand, TOutput>>();

        return await handler.HandleAsync((TCommand)command);
    }

    public override object Handle(object command, IServiceProvider serviceProvider)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TOutput>>();

        return handler.Handle((TCommand)command);
    }
}

