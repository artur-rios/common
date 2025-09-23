using ProtoBuf;

namespace ArturRios.Common.ProtoBuf;

public static class Extensions
{
    public static byte[] ToProtoBufBytes<T>(this T source)
    {
        using var stream = new MemoryStream();

        Serializer.Serialize(stream, source);

        return stream.ToArray();
    }

    public static T ToProtoBufObject<T>(this byte[] source)
    {
        using var stream = new MemoryStream(source);

        return Serializer.Deserialize<T>(stream);
    }
}
