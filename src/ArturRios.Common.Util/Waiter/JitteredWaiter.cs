namespace ArturRios.Common.Util.Waiter;

public class JitteredWaiter(int maxRetryCount)
{
    private const int FixedWaitDelay = 500;
    public int MaxRetryCount { get; set; } = maxRetryCount;
    private int Count { get; set; }

    public bool CanRetry => Count < MaxRetryCount;

    public async Task Wait()
    {
        if (Count >= MaxRetryCount)
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
            await Task.Delay(FixedWaitDelay + backoffPeriodMs / 2 + new System.Random().Next(0, backoffPeriodMs / 2));
        }
    }
}
