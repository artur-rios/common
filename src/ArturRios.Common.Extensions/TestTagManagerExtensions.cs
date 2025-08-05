using System.Collections;

namespace ArturRios.Common.Extensions;

public static class TestTagManagerExtensions
{
    public static void PrintContents(this IEnumerable enumerable)
    {
        foreach (var item in enumerable)
        {
            if (item == null)
            {
                Console.WriteLine("null");

                continue;
            }

            var type = item.GetType();

            if (type.IsPrimitive || item is string || item is decimal)
            {
                Console.WriteLine(item);
            }
            else
            {
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item, null);
                    Console.WriteLine($"{prop.Name}: {value}");
                }
            }
        }
    }
}
