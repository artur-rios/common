namespace ArturRios.Common.Util.Condition;

public class ConditionFailedException(string[] errors) : Exception($"A total of {errors.Length} conditions failed")
{
    // ReSharper disable once UnusedMember.Global
    // Reason: This class is meant to be used in other projects
    public readonly string[] Errors = errors;
}
