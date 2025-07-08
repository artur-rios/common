// ReSharper disable InconsistentNaming
// Reason: these are not test methods

using System.ComponentModel;

namespace ArturRios.Common.Extensions;

public static class EnumExtensions
{
    public static string? GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .Cast<DescriptionAttribute>()
            .FirstOrDefault();
        
        return attribute?.Description;
    }
}
