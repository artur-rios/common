namespace ArturRios.Common.Output;

public class ProcessOutput(IList<string>? errors = null)
{
    public IList<string> Errors { get; } = errors ?? [];
    public bool Success => !Errors.Any();
}
