using System.ComponentModel;

namespace ArturRios.Common.Aws.Lambda.Config;

public enum LogType
{
    [Description("[INFO]")] Info,
    [Description("[WARN]")] Warn,
    [Description("[ERROR]")] Error
}
