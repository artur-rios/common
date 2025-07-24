// ReSharper disable InconsistentNaming
// Reason: these are not test methods

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
}
