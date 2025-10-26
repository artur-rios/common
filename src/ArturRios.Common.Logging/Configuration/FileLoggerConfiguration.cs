namespace ArturRios.Common.Logging.Configuration;

public class FileLoggerConfiguration : LoggerConfiguration
{
    public required string ApplicationName { get; set; }
    public LogFolderScheme FolderScheme { get; set; } = LogFolderScheme.ByMonth;
    public LogSplitLevel FileSplitLevel { get; set; } = LogSplitLevel.Day;
    public string? FilePath { get; set; }
}
