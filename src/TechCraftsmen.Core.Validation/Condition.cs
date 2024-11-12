using TechCraftsmen.Core.Output;

namespace TechCraftsmen.Core.Validation;

public class Condition
{
    private bool _expression;

    private readonly HashSet<string> _failedConditions = [];
    
    public static Condition Create => new ();

    public string[] FailedConditions => _failedConditions.ToArray();

    public bool IsSatisfied => _failedConditions.Count == 0;

    public Condition If(bool expression)
    {
        _expression = expression;

        return this;
    }

    public Condition IfNot(bool expression)
    {
        _expression = !expression;

        return this;
    }

    public Condition FailsWith(string error)
    {
        if (!_expression)
        {
            _failedConditions.Add(error);
        }

        return this;
    }

    public void ThrowIfNotSatisfied()
    {
        if (IsSatisfied)
        {
            return;
        }

        throw new ConditionFailedException(FailedConditions);
    }

    public ProcessOutput ToProcessOutput()
    {
        return new ProcessOutput(_failedConditions.ToList());
    }
}
