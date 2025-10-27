using System.Runtime.CompilerServices;
using ArturRios.Common.Logging.Configuration;
using ArturRios.Common.Logging.Factories;
using ArturRios.Common.Logging.Interfaces;

namespace ArturRios.Common.Logging.Loggers;

public class FileLogger(FileLoggerConfiguration configuration) : IInternalLogger
{
    private const string DefaultLogFolder = "log";
    private const string FileExtension = ".log";

    public void Trace(string message, string filePath, string methodName)
    {
        Write(LogLevel.Trace, filePath, methodName, message);
    }

    public void Debug(string message, string filePath, string methodName)
    {
        Write(LogLevel.Debug, filePath, methodName, message);
    }

    public void Info(string message, string filePath, string methodName)
    {
        Write(LogLevel.Information, filePath, methodName, message);
    }

    public void Warn(string message, string filePath, string methodName)
    {
        Write(LogLevel.Warning, filePath, methodName, message);
    }

    public void Error(string message, string filePath, string methodName)
    {
        Write(LogLevel.Error, filePath, methodName, message);
    }

    public void Exception(Exception exception, string filePath, string methodName)
    {
        Write(LogLevel.Exception, filePath, methodName, exception.Message);
    }

    public void Critical(string message, string filePath, string methodName)
    {
        Write(LogLevel.Critical, filePath, methodName, message);
    }

    public void Fatal(string message, string filePath, string methodName)
    {
        Write(LogLevel.Fatal, filePath, methodName, message);
    }

    private void Write(LogLevel level, string filePath, string methodName, string message)
    {
        var path = BuildFullPath();

        CreateDirectoryIfNotExists(path);

        File.AppendAllText(path, LogEntryFactory.Create(level, filePath, methodName, message));
    }

    private static void CreateDirectoryIfNotExists(string path)
    {
        var directory = Path.GetDirectoryName(path);

        if (directory is not null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private string BuildFullPath()
    {
        var timestamp = DateTime.UtcNow;
        var folderPath = BuildFolderPath(timestamp);
        var fileName = BuildFileName(timestamp);

        return Path.Combine(folderPath, $"{fileName}{FileExtension}");
    }

    private string BuildFolderPath(DateTime timestamp)
    {
        var basePath = configuration.FilePath ?? Path.Combine(AppContext.BaseDirectory, DefaultLogFolder);
        var path = basePath;

        switch (configuration.FolderScheme)
        {
            case LogFolderScheme.AllInOne:
                break;
            case LogFolderScheme.ByYear:
                path = Path.Combine(path, timestamp.Year.ToString());
                break;
            case LogFolderScheme.ByMonth:
                path = Path.Combine(path, timestamp.Year.ToString(), timestamp.Month.ToString("D2"));
                break;
            case LogFolderScheme.ByDay:
                path = Path.Combine(path, timestamp.Year.ToString(), timestamp.Month.ToString("D2"), timestamp.Day.ToString("D2"));
                break;
            case LogFolderScheme.ByHour:
                path = Path.Combine(path, timestamp.Year.ToString(), timestamp.Month.ToString("D2"), timestamp.Day.ToString("D2"), timestamp.Hour.ToString("D2"));
                break;
            case LogFolderScheme.ByRequest:
                var requestId = Guid.NewGuid().ToString();
                path = Path.Combine(path, requestId);
                break;
            default:
                throw new ArgumentOutOfRangeException(configuration.FolderScheme.ToString());
        }

        return path;
    }

    private string BuildFileName(DateTime timestamp)
    {
        return configuration.FileSplitLevel switch
        {
            LogSplitLevel.Request => configuration.ApplicationName,
            LogSplitLevel.Year => $"{configuration.ApplicationName}_{timestamp:yyyy}",
            LogSplitLevel.Month => $"{configuration.ApplicationName}_{timestamp:yyyy_MM}",
            LogSplitLevel.Day => $"{configuration.ApplicationName}_{timestamp:yyyy_MM_dd}",
            LogSplitLevel.Hour => $"{configuration.ApplicationName}_{timestamp:yyyy_MM_dd_HH}",
            _ => throw new ArgumentOutOfRangeException(configuration.FileSplitLevel.ToString())
        };
    }
}
