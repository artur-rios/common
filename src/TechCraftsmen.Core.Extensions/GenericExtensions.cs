// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

using Newtonsoft.Json;

namespace TechCraftsmen.Core.Extensions;

public static class GenericExtensions
{
    public static T? Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
            
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}
