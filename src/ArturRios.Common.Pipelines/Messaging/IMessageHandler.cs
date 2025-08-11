using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Messaging;

public interface IMessageHandler<in T>
{
    Task<ProcessOutput> HandleAsync(T message);
}
