using ArturRios.Common.Output;
using FluentValidation.Results;

namespace ArturRios.Common.Extensions;

public static class FluentValidationExtensions
{
    public static DataOutput<T> ToDataOutput<T>(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

        var output = new DataOutput<T>();

        if (validationErrors.Length != 0)
        {
            output.AddErrors(validationErrors);
        }

        return output;
    }

    public static ProcessOutput ToProcessOutput(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

        var output = new ProcessOutput();

        if (validationErrors.Length != 0)
        {
            output.AddErrors(validationErrors);
        }

        return output;
    }
}
