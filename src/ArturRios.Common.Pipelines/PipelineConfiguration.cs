using ArturRios.Common.Pipelines.Interfaces;

namespace ArturRios.Common.Pipelines;

public class PipelineConfiguration : IPipelineConfiguration
{
    public int MaxRetryCount { get; set; }
}
