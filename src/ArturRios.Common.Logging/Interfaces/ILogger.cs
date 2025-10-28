using System.Runtime.CompilerServices;

namespace ArturRios.Common.Logging.Interfaces;

public interface ILogger
{
    string? TraceId { get; set; }

    void Trace(string message, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
    void Debug(string message, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
    void Info(string message, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
    void Warn(string message, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
    void Error(string message, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
    void Exception(Exception exception, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
    void Critical(string message, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
    void Fatal(string message, [CallerFilePath] string filePath = "unknown", [CallerMemberName] string methodName = "unknown");
}
