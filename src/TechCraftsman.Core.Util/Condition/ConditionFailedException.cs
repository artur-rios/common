namespace TechCraftsman.Core.Util.Condition;

public class ConditionFailedException(string[] errors) : Exception($"A total of {errors.Length} conditions failed")
{
    public readonly string[] Errors = errors;
}
