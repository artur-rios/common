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
