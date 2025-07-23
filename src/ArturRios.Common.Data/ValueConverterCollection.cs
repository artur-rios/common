// ReSharper disable InconsistentNaming
// Reason: these are not test methods

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArturRios.Common.Data;

public static class ValueConverterCollection
{
    public static ValueConverter<T, string> EnumIntToString<T>() where T : Enum
    {
        var converter = new ValueConverter<T, string>(
            v => v.ToString(),
            v => (T)Enum.Parse(typeof(T), v)
        );

        return converter;
    }
}