using ArturRios.Common.ProtoBuf.Tests.Mock;

namespace ArturRios.Common.ProtoBuf.Tests;

public class ExtensionsTests
{
    [Fact]
    public void Should_SerializeAndDeserialize()
    {
        var person = new Person { Id = 123, Name = "John Doe", Email = "john.doe@example.com" };

        var serializedPerson = person.ToProtoBufBytes();
        var deserializedPerson = serializedPerson.ToProtoBufObject<Person>();

        Assert.Equal(person.Id, deserializedPerson.Id);
        Assert.Equal(person.Name, deserializedPerson.Name);
        Assert.Equal(person.Email, deserializedPerson.Email);
    }
}
