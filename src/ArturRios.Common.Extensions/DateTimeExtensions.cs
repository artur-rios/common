namespace ArturRios.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime RemoveMilliseconds(this DateTime dateTime) =>
        new(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            dateTime.Second,
            dateTime.Kind);
}
