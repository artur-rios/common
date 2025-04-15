using TechCraftsmen.Core.Extensions.Tests.Mock;

namespace TechCraftsmen.Core.Extensions.Tests;

public class EnumExtensionsTests
{
    [Fact]
    public void Should_GetDescription()
    {
        var description = TestEnum.One.GetDescription();
        
        Assert.Equal("One", description);
    }
    
    [Fact]
    public void Should_ReturnNull_IfEnumValueHasNoDescription()
    {
        var description = TestEnum.Three.GetDescription();
        
        Assert.Null(description);
    }
    
    [Fact]
    public void Should_ReturnNull_IfEnumValueNotFound()
    {
        var description = ((TestEnum) 100).GetDescription();
        
        Assert.Null(description);
    }
}
