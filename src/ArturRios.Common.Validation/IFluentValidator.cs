// ReSharper disable InconsistentNaming
// Reason: these are not test methods

using FluentValidation;

namespace ArturRios.Common.Validation;

public interface IFluentValidator<in T> : IValidator<T>
{
    string[] ValidateAndReturnErrors(T model);
}
