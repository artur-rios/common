// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

using ArturRios.Common.Output;
using FluentValidation.Results;

namespace ArturRios.Common.Extensions;

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
