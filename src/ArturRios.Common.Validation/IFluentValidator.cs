// ReSharper disable InconsistentNaming
// Reason: these are not test methods

namespace ArturRios.Common.Validation;

public interface IFluentValidator<in T>
{
    string[] ValidateAndReturnErrors(T model);
}
