using System.ComponentModel;

namespace ArturRios.Common.Logging;

public enum CustomLogLevel
{
    [Description("TRACE")]
    Trace = 0,

    [Description("DEBUG")]
    Debug = 1,

    [Description("INFO")]
    Information = 2,

    [Description("WARN")]
    Warning = 3,

    [Description("ERROR")]
    Error = 4,

    [Description("EXCEPTION")]
    Exception = 5,

    [Description("CRITICAL")]
    Critical = 6,

    [Description("FATAL")]
    Fatal = 7
}
