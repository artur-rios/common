using System.Collections;

namespace ArturRios.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsEmpty(this IEnumerable? enumerable)
    {
        switch (enumerable)
        {
            case null:
                return true;
            case ICollection collection:
                return collection.Count == 0;
            default:
            {
                var enumerator = enumerable.GetEnumerator();
                try
                {
                    return !enumerator.MoveNext();
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }
    }

    public static bool IsNotEmpty(this IEnumerable? enumerable) => !IsEmpty(enumerable);

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
