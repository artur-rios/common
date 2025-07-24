// ReSharper disable UnusedMember.Global
// Reason: This class is meant to be used in other projects

// ReSharper disable InconsistentNaming
// Reason: these are not test methods

using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace ArturRios.Common.Extensions;

public static class ObjectExtensions
{
    public static StringContent ToJsonStringContent(this object @object)
    {
        var json = JsonConvert.SerializeObject(@object);

        return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    }

    public static Dictionary<string, object> NonNullPropertiesToDictionary(this object @object)
    {
        Dictionary<string, object> dictionary = new();

        foreach (var propertyInfo in @object.GetType().GetProperties())
        {
            var value = propertyInfo.GetValue(@object);

            if (value is not null)
            {
                dictionary[propertyInfo.Name] = value;
            }
        }

        return dictionary;
    }

    public static Dictionary<string, object?> PropertiesToDictionary(this object @object)
    {
        Dictionary<string, object?> dictionary = new();

        foreach (var propertyInfo in @object.GetType().GetProperties())
        {
            var value = propertyInfo.GetValue(@object);

            dictionary[propertyInfo.Name] = value;
        }

        return dictionary;
    }
}
