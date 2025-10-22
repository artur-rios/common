using System.Web;
using ArturRios.Common.Attributes;
using ArturRios.Common.Aws.Lambda.Config;
using ArturRios.Common.Aws.Lambda.Logging.Interfaces;
using ArturRios.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Aws.Lambda.Logging;

[InjectionValidator(ServiceLifetime.Singleton)]
public class LambdaLogTracerSingleton : ILambdaLogTracerSingleton
{
    internal LambdaLogTracerSingleton() => Info($"Starting {nameof(LambdaLogTracerSingleton)}", 0);

    public string GetTraceParams(Dictionary<string, string> traceParams, string key) =>
        traceParams.TryGetValue(key, out var value) ? value : string.Empty;

    public T GetTraceParams<T>(Dictionary<string, string> traceParams, string key) =>
        traceParams.TryGetValue(key, out var value) ? (T)Convert.ChangeType(value, typeof(T)) : default!;

    public void AddTraceParams(Dictionary<string, string> traceParams, string key, string value) =>
        traceParams.TryAdd(key, value);

    public string GetLogStream(int? currentTrace = null, Dictionary<string, string>? traceParams = null)
    {
        var logGroup = Environment.GetEnvironmentVariable("AWS_LAMBDA_LOG_GROUP_NAME") ?? "NotSet";
        var logStream = Environment.GetEnvironmentVariable("AWS_LAMBDA_LOG_STREAM_NAME") ?? "NotSet";
        var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "NotSet";

        return
            $"https://{region}.console.aws.amazon.com/cloudwatch/home?region={region}#logsV2:log-groups/log-group/{HttpUtility.UrlEncode(logGroup)}/log-events/{HttpUtility.UrlEncode(logStream)}$FilterPattern$3D$2522{HttpUtility.UrlEncode(GetTraceParams(currentTrace, traceParams ?? []))}$2522";
    }

    public string GetBucketKeyPath(string bucketName, string bucketKey)
    {
        var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "NotSet";

        return $"https://s3.console.aws.amazon.com/s3/object/{bucketName}?region={region}&prefix={bucketKey}";
    }

    public void Info(object message, int? currentTrace = null, Dictionary<string, string>? traceParams = null) =>
        Console.WriteLine(GetBasicLog(message, LogType.Info, currentTrace, traceParams ?? []));

    public void Error(object message, Exception exception, int? currentTrace = null,
        Dictionary<string, string>? traceParams = null) =>
        LogException(message, exception, currentTrace, traceParams ?? []);

    public void Error(object message, int? currentTrace = null, Dictionary<string, string>? traceParams = null) =>
        Console.WriteLine(GetBasicLog(message, LogType.Error, currentTrace, traceParams ?? []));

    public void Error(Exception exception, int? currentTrace = null, Dictionary<string, string>? traceParams = null) =>
        LogException(exception, currentTrace, traceParams ?? []);

    public void Warn(object message, int? currentTrace = null, Dictionary<string, string>? traceParams = null) =>
        Console.WriteLine(GetBasicLog(message, LogType.Warn, currentTrace, traceParams ?? []));

    private static string GetBasicLog(object message, LogType logType, int? currentTrace,
        Dictionary<string, string> traceParams) =>
        $"{GetTraceParams(currentTrace, traceParams)} {logType.GetDescription()} - Message: {message}";

    private static string GetTraceParams(int? currentTrace, Dictionary<string, string> traceParams) =>
        $"[{currentTrace}] {string.Join(" ", traceParams.Select(param => string.Concat("[", param.Key, ":", param.Value, "]")))}";

    private static void LogException(object message, Exception exception, int? currentTrace = null,
        Dictionary<string, string>? traceParams = null) => Console.Error.WriteLine(
        $"{GetBasicLog(message, LogType.Error, currentTrace, traceParams ?? [])} | Exception: {exception.ToLogLine(out _)}");

    private static void
        LogException(Exception exception, int? currentTrace = null, Dictionary<string, string>? traceParams = null) =>
        Console.Error.WriteLine(
            $"{GetBasicLog($"Exception message: {exception.Message}", LogType.Error, currentTrace, traceParams ?? [])} | Exception: {exception.ToLogLine(out _)}");
}
