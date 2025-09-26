namespace ArturRios.Common.Util;

public class Retry
{
    private int _maxAttempts;
    private int _delayMilliseconds;

    public static Retry New => new();

    public Retry MaxAttempts(int maxAttempts)
    {
        _maxAttempts = maxAttempts;

        return this;
    }

    public Retry DelayMilliseconds(int delayMilliseconds)
    {
        _delayMilliseconds = delayMilliseconds;

        return this;
    }

    public void Execute(Action action)
    {
        while (true)
        {
            try
            {
                action();
                break;
            }
            catch
            {
                if (_maxAttempts-- <= 0)
                {
                    throw;
                }

                if (_delayMilliseconds > 0)
                {
                    Thread.Sleep(_delayMilliseconds);
                }
            }
        }
    }

    public T Execute<T>(Func<T> func)
    {
        while (true)
        {
            try
            {
                return func();
            }
            catch
            {
                if (_maxAttempts-- <= 0)
                {
                    throw;
                }

                if (_delayMilliseconds > 0)
                {
                    Thread.Sleep(_delayMilliseconds);
                }
            }
        }
    }
}
