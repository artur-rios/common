namespace ArturRios.Common.Output;

public class ProcessOutput
{
    public List<string> Messages { get; } = [];
    public List<string> Errors { get; } = [];
    public bool Success => Errors.Count == 0;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ProcessOutput New => new();

    public void AddError(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            return;
        }

        Errors.Add(error);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        var filteredErrors = errors.Where(e => !string.IsNullOrWhiteSpace(e)).ToList();

        if (filteredErrors.Count == 0)
        {
            return;
        }

        Errors.AddRange(filteredErrors);
    }

    public void AddMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        Messages.Add(message);
    }

    public void AddMessages(IEnumerable<string> messages)
    {
        Messages.AddRange(messages.Where(e => !string.IsNullOrWhiteSpace(e)).ToList());
    }

    public ProcessOutput WithError(string error)
    {
        AddError(error);

        return this;
    }

    public ProcessOutput WithErrors(IEnumerable<string> errors)
    {
        AddErrors(errors);

        return this;
    }

    public ProcessOutput WithMessage(string message)
    {
        AddMessage(message);

        return this;
    }

    public ProcessOutput WithMessages(IEnumerable<string> messages)
    {
        AddMessages(messages);

        return this;
    }
}
