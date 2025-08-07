using ArturRios.Common.Pipelines.Commands.Interfaces;

namespace ArturRios.Common.Pipelines.Commands.Queues;

// TODO
public class CustomCommandQueue : ICommandQueue
{
    public void Enqueue(object command) => throw new NotImplementedException();

    public Task Flush() => throw new NotImplementedException();

    public void Schedule(object command, DateTime dueDate) => throw new NotImplementedException();
}
