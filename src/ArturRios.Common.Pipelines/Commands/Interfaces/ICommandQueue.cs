namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommandQueue
{
    void Enqueue(object command);
    Task Flush();
    void Schedule(object command, DateTime dueDate);
}
