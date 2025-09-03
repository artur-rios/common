// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: these methods are meant to be used in other projects

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArturRios.Common.Data.Util;

public static class ValueConverterCollection
{
    public static ValueConverter<int, string> EnumIntToString<T>() where T : Enum =>
        new(
            v => Enum.GetName(typeof(T), v) ?? string.Empty,
            v => (int)Enum.Parse(typeof(T), v)
        );
}
