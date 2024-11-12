namespace TechCraftsmen.Core.Output;

public class ProcessOutput(IList<string>? errors = null)
{
    public IList<string> Errors { get; } = errors ?? [];
    public bool Success => !Errors.Any();
}
