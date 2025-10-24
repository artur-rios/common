namespace ArturRios.Common.Logging;

public class SimpleFileLogger
{
    private const string FileExtension = ".log";
    private readonly string _fullPath;

    public SimpleFileLogger(string fileName, string path)
    {
        _fullPath = Path.Combine(path, $"{fileName}{FileExtension}");

        var directory = Path.GetDirectoryName(_fullPath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }
    }

    public void Info(string message)
    {
        File.AppendAllText(_fullPath, $"INFO | {DateTime.Now} | {message}{Environment.NewLine}");
    }

    public void Debug(string message)
    {
        File.AppendAllText(_fullPath, $"DEBUG | {DateTime.Now} | {message}{Environment.NewLine}");
    }

    public void Warn(string message)
    {
        File.AppendAllText(_fullPath, $"WARN | {DateTime.Now} | {message}{Environment.NewLine}");
    }

    public void Error(string message)
    {
        File.AppendAllText(_fullPath, $"ERROR | {DateTime.Now} | {message}{Environment.NewLine}");
    }

    public void Exception(Exception exception)
    {
        File.AppendAllText(_fullPath, $"EXCEPTION | {DateTime.Now} | {exception.Message}{Environment.NewLine}");
    }
}
