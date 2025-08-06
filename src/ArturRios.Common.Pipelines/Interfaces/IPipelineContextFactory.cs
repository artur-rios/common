using Microsoft.EntityFrameworkCore;

namespace ArturRios.Common.Pipelines.Interfaces;

public interface IPipelineContextFactory
{
    DbContext GetContext();
}
