using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines;

public interface ICommand<TOutput> where TOutput : CommandOutput;
