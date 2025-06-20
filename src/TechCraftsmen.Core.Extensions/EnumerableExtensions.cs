using System.Collections;

namespace TechCraftsmen.Core.Extensions;

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
                        disposable.Dispose();
                }
            }
        }
    }
    
    public static bool IsNotEmpty(this IEnumerable? enumerable)
    {
        return !IsEmpty(enumerable);
    }
}
