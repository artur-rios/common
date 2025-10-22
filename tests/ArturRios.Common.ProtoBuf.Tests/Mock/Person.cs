using ProtoBuf;

namespace ArturRios.Common.ProtoBuf.Tests.Mock;

[ProtoContract]
public class Person
{
    [ProtoMember(1, IsRequired = true)] public int Id { get; set; }

    [ProtoMember(2, IsRequired = true)] public required string Name { get; set; }

    [ProtoMember(3, IsRequired = true)] public required string Email { get; set; }
}
