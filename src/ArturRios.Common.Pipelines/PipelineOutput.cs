using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines;

public class PipelineOutput : ProcessOutput
{
    public static PipelineOutput New => new();

    public object? Result { get; private set; } = null;

    public PipelineOutput WithMessage(string message)
    {
        AddMessage(message);

        return this;
    }

    public PipelineOutput WithMessages(IEnumerable<string> messages)
    {
        AddMessages(messages);

        return this;
    }

    public PipelineOutput WithError(string error)
    {
        AddError(error);

        return this;
    }

    public PipelineOutput WithErrors(IEnumerable<string> errors)
    {
        AddErrors(errors);

        return this;
    }

    public PipelineOutput WithResult(object? result)
    {
        Result = result;

        return this;
    }
}
