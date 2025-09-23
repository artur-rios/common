using ArturRios.Common.ProtoBuf.Tests.Mock;

namespace ArturRios.Common.ProtoBuf.Tests;

public class ProtoBufServiceTests
{
    [Fact]
    public void Should_GenerateProto()
    {
        var testProjectDirectory =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));

        var protoTypes = new List<Type> { typeof(Person) };

        var proto = ProtoBufService.GenerateDefinition(protoTypes, "ProtoBufTest", testProjectDirectory);

        Assert.Contains("message Person", proto);
        Assert.Contains("required int32 Id = 1;", proto);
        Assert.Contains("required string Name = 2;", proto);
        Assert.Contains("required string Email = 3;", proto);
    }
}
