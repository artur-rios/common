namespace ArturRios.Common.Pipelines.Waiter;

public class JitteredWaiter(int maxRetryCount)
{
    private int Count { get; set; }

    private const int FixedWaitDelay = 500;

    public bool CanRetry => Count < maxRetryCount;

    public async Task Wait()
    {
        if (Count >= maxRetryCount)
        {
            throw new MaxRetriesReachedException();
        }

        var currentRetryAttempt = Count++;

        if (currentRetryAttempt == 0)
        {
            await Task.Delay(FixedWaitDelay);
        }
        else
        {
            var backoffPeriodMs = Convert.ToInt32(Math.Pow(2, currentRetryAttempt) * 1000) - FixedWaitDelay;
            await Task.Delay(FixedWaitDelay + backoffPeriodMs / 2 + new Random().Next(0, backoffPeriodMs / 2));
        }
    }
}
