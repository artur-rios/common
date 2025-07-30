using System.Web;
using ArturRios.Common.Attributes;
using ArturRios.Common.Aws.Lambda.Config;
using ArturRios.Common.Aws.Lambda.Logging.Interfaces;
using ArturRios.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Aws.Lambda.Logging;

[InjectionValidator(ServiceLifetime.Scoped)]
public class LambdaLogTracer : ILambdaLogTracer
{
    private readonly string _logStream;
    private readonly string _logGroup;
    private readonly string _region;
    private readonly int _currentTrace;
    private readonly Dictionary<string, string> _traceParams;
    private static int _traceCounter;

    internal LambdaLogTracer()
    {
        _traceCounter++;
        _currentTrace = _traceCounter;

        _logGroup = Environment.GetEnvironmentVariable("AWS_LAMBDA_LOG_GROUP_NAME") ?? "NotSet";
        _logStream = Environment.GetEnvironmentVariable("AWS_LAMBDA_LOG_STREAM_NAME") ?? "NotSet";
        _region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "NotSet";

        _traceParams = [];

        Info($"Starting {nameof(LambdaLogTracer)} | Current trace: {_currentTrace}");
    }

    private string GetBasicLog(object message, LogType logType) =>
        $"{GetTraceParams()} {logType.GetDescription()} - Message: {message}";

    private string GetTraceParams() =>
        $"{_currentTrace} {string.Join(" ", _traceParams.Select(param => string.Concat("[", param.Key, ":", param.Value, "]")))}";

    private void LogException(object message, Exception exception) =>
        Console.Error.WriteLine($"{GetBasicLog(message, LogType.Error)} | Exception: {exception.ToLogLine(out _)}");

    private void LogException(Exception exception) => Console.Error.WriteLine(
        $"{GetBasicLog($"Exception message: {exception.Message}", LogType.Error)} | Exception: {exception.ToLogLine(out _)}");

    public string GetTraceParams(string key) => _traceParams.TryGetValue(key, out var value) ? value : string.Empty;

    public T? GetTraceParams<T>(string key) where T : struct
    {
        if (!_traceParams.TryGetValue(key, out var value))
        {
            return null;
        }

        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        return (T?)Convert.ChangeType(value, targetType);
    }

    public void AddTraceParams(string key, string value) => _traceParams.TryAdd(key, value);

    public string GetLogStream() =>
        $"https://{_region}.console.aws.amazon.com/cloudwatch/home?region={_region}#logsV2:log-groups/log-group/{HttpUtility.UrlEncode(_logGroup)}/log-events/{HttpUtility.UrlEncode(_logStream)}$FilterPattern$3D$2522{HttpUtility.UrlEncode(GetTraceParams())}$2522";

    public string GetBucketKeyPath(string bucketName, string bucketKey) =>
        $"https://s3.console.aws.amazon.com/s3/object/{bucketName}?region={_region}&prefix={bucketKey}";

    public void Info(object message) => Console.WriteLine(GetBasicLog(message, LogType.Info));

    public void Error(object message, Exception exception) => LogException(message, exception);

    public void Error(object message) => Console.WriteLine(GetBasicLog(message, LogType.Error));

    public void Error(Exception exception) => LogException(exception);

    public void Warn(object message) => Console.WriteLine(GetBasicLog(message, LogType.Warn));
}
