using System.Collections;

namespace TechCraftsmen.Core.Extensions;

public static class EnumerableExtensions
{
    public static bool Empty(this IEnumerable enumerable)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        // Reason: Should test even for non-nullable types
        if (enumerable is null)
        {
            return true;
        }
        
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