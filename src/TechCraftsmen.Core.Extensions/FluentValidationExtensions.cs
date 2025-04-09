using FluentValidation.Results;
using TechCraftsmen.Core.Output;

namespace TechCraftsmen.Core.Extensions;

public static class FluentValidationExtensions
{
    public static DataOutput<T> ToDataOutput<T>(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

        return new DataOutput<T>(default, validationErrors, validationResult.IsValid);
    }
    
    public static ProcessOutput ToProcessOutput(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

        return new ProcessOutput(validationErrors);
    }
}
