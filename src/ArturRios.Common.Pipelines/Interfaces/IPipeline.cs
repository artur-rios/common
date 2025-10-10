using ArturRios.Common.Pipelines.Commands.Interfaces;
using ArturRios.Common.Pipelines.Queries;

namespace ArturRios.Common.Pipelines.Interfaces;

public interface IPipeline
{
    Task<PipelineOutput> ExecuteCommandAsync(object command);

    Task<PipelineOutput> ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteQueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
        where TResult : class;

    Task<PipelineOutput> ExecuteQueryAsync(object query);
}
