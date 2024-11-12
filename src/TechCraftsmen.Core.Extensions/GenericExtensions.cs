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
