using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands.IO;
using ArturRios.Common.Pipelines.Queries;

namespace ArturRios.Common.Pipelines.Interfaces;

public interface IPipeline
{
    Task<PipelineOutput> ExecuteCommand(object command);

    Task<PipelineOutput> ExecuteCommand<TCommand, TOutput>(TCommand command)
        where TCommand : ICommand<TOutput> where TOutput : CommandOutput;

    Task<PipelineOutput<T>> ExecuteCommand<TCommand, TInput, TOutput, T>(TCommand command)
        where TCommand : ICommand<TInput, TOutput, T>
        where TInput : CommandInput
        where TOutput : CommandOutput<T>;

    Task<TResult> ExecuteQuery<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
        where TResult : class;

    Task<PipelineOutput> ExecuteQuery(object query);
}
