namespace ArturRios.Common.Extensions;

public static class ExceptionExtensions
{
    public static string ToLogLine(this Exception exception, out Guid traceId)
    {
        traceId = Guid.NewGuid();

        return $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | TraceId: {traceId} | Exception: {exception.GetType().Name} | Message: {exception.Message} | StackTrace: {exception.StackTrace}";
    }
}
