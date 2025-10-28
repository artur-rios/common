namespace ArturRios.Common.Logging.Interfaces;

public interface IInternalLogger
{
    void Trace(string message, string filePath, string methodName);
    void Debug(string message, string filePath, string methodName);
    void Info(string message, string filePath, string methodName);
    void Warn(string message, string filePath, string methodName);
    void Error(string message, string filePath, string methodName);
    void Exception(string message, string filePath, string methodName);
    void Critical(string message, string filePath, string methodName);
    void Fatal(string message, string filePath, string methodName);
}
