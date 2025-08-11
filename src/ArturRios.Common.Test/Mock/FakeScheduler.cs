using ArturRios.Common.Pipelines.Messaging;

namespace ArturRios.Common.Test.Mock;

public class FakeScheduler(int waitTimeInMinutes = 2)
{
    public async Task CreateSchedule<THandler, TMessage>(TMessage command, THandler handler)
        where THandler : IMessageHandler<TMessage>
    {
        await Task.Delay(TimeSpan.FromMinutes(waitTimeInMinutes));

        await handler.HandleAsync(command);
    }
}
