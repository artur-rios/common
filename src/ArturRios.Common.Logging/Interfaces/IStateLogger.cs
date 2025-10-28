namespace ArturRios.Common.Logging.Interfaces;

public interface IStateLogger
{
    string? TraceId { get; set; }

    void Trace(string message, object? state = null);
    void Debug(string message, object? state = null);
    void Info(string message, object? state = null);
    void Warn(string message, object? state = null);
    void Error(string message, object? state = null);
    void Exception(Exception exception, object? state = null);
    void Critical(string message, object? state = null);
    void Fatal(string message, object? state = null);
}
