namespace ArturRios.Common.Aws.Lambda.Logging.Interfaces;

public interface ILambdaLogTracerSingleton
{
    string GetTraceParams(Dictionary<string, string> traceParams, string key);
    T GetTraceParams<T>(Dictionary<string, string> traceParams, string key);
    void AddTraceParams(Dictionary<string, string> traceParams, string key, string value);
    string GetLogStream(int? currentTrace = null, Dictionary<string, string>? traceParams = null);
    string GetBucketKeyPath(string bucketName, string bucketKey);
    void Info(object message, int? currentTrace = null, Dictionary<string, string>? traceParams = null);
    void Error(object message, Exception exception, int? currentTrace = null, Dictionary<string, string>? traceParams = null);
    void Error(object message, int? currentTrace = null, Dictionary<string, string>? traceParams = null);
    void Error(Exception exception, int? currentTrace = null, Dictionary<string, string>? traceParams = null);
    void Warn(object message, int? currentTrace = null, Dictionary<string, string>? traceParams = null);
}
