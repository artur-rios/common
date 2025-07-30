namespace ArturRios.Common.Aws.Lambda.Logging.Interfaces;

public interface ILambdaLogTracer
{
    string GetTraceParams(string key);
    T? GetTraceParams<T>(string key) where T : struct;
    void AddTraceParams(string key, string value);
    string GetLogStream();
    string GetBucketKeyPath(string bucketName, string bucketKey);
    void Info(object message);
    void Error(object message, Exception exception);
    void Error(object message);
    void Error(Exception exception);
    void Warn(object message);
}
