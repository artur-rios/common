using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands.IO;
using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Pipelines.Commands;

internal abstract class CommandHandlerWrapper
{
    public abstract Task<object> ExecuteAsync(object command, IServiceProvider serviceProvider);
}

internal class CommandHandlerWrapper<TCommand, TOutput> : CommandHandlerWrapper
    where TCommand : ICommand<TOutput>
    where TOutput : CommandOutput
{
    public override async Task<object> ExecuteAsync(object command, IServiceProvider serviceProvider)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TOutput>>();

        return await handler.HandleAsync((TCommand)command);
    }
}

internal class CommandHandlerWrapper<TCommand, TInput, TOutput> : CommandHandlerWrapper
    where TCommand : ICommand<TInput, TOutput>
    where TInput : CommandInput
    where TOutput : CommandOutput
{
    public override async Task<object> ExecuteAsync(object command, IServiceProvider serviceProvider)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TInput, TOutput>>();

        return await handler.HandleAsync((TCommand)command);
    }
}
