// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

// ReSharper disable InconsistentNaming
// Reason: these are not test methods

using Newtonsoft.Json;

namespace ArturRios.Common.Extensions;

public static class GenericExtensions
{
    public static T? Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
            
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}
