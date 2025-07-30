namespace ArturRios.Common.Extensions;

public static class ComparisonExtensions
{
    public static bool In<T>(this T self, params T[] range) => range.Any(t => t?.Equals(self) ?? self == null);
    public static bool NotIn<T>(this T self, params T[] range) => !self.In(range);
}
