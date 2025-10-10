using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands;

namespace ArturRios.Common.Pipelines;

public class CommandPipeline(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public CommandOutput Execute<TOutput>(ICommand command) where TOutput : CommandOutput
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();

        var wrapperType = typeof(CommandHandlerWrapper<,>).MakeGenericType(commandType, typeof(TOutput));
        var wrapperObj = Activator.CreateInstance(wrapperType) as CommandHandlerWrapper
                         ?? throw new InvalidOperationException($"Unable to create command handler wrapper for {commandType.Name} and output {typeof(TOutput).Name}");

        var resultObj = wrapperObj.Handle(command, _serviceProvider);

        return resultObj as CommandOutput ?? throw new InvalidOperationException($"Handler returned unexpected result for {commandType.Name} and output {typeof(TOutput).Name}");
    }

    public async Task<CommandOutput> ExecuteAsync<TOutput>(ICommand command) where TOutput : CommandOutput
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();

        var wrapperType = typeof(CommandHandlerWrapper<,>).MakeGenericType(commandType, typeof(TOutput));
        var wrapperObj = Activator.CreateInstance(wrapperType) as CommandHandlerWrapper
                         ?? throw new InvalidOperationException($"Unable to create command handler wrapper for {commandType.Name} and output {typeof(TOutput).Name}");

        var resultObj = await wrapperObj.HandleAsync(command, _serviceProvider);

        return resultObj as CommandOutput ?? throw new InvalidOperationException($"Handler returned unexpected result for {commandType.Name} and output {typeof(TOutput).Name}");
    }
}
