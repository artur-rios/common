using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Commands.IO;
using ArturRios.Common.Pipelines.Queries;

namespace ArturRios.Common.Pipelines.Interfaces;

public interface IPipeline
{
    Task<PipelineOutput> ExecuteCommandAsync(object command);

    Task<PipelineOutput> ExecuteCommandAsync<TCommand, TOutput>(TCommand command)
        where TCommand : ICommand<TOutput> where TOutput : CommandOutput;

    Task<PipelineOutput<T>> ExecuteCommandAsync<TCommand, TInput, TOutput, T>(TCommand command)
        where TCommand : ICommand<TInput, TOutput, T>
        where TInput : CommandInput
        where TOutput : CommandOutput<T>;

    Task<TResult> ExecuteQueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
        where TResult : class;

    Task<PipelineOutput> ExecuteQueryAsync(object query);
}
