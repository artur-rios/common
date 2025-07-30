namespace ArturRios.Common.Util.Tests;

public class ConditionTests
{
    [Fact]
    public void Should_Succeed()
    {
        var output = Condition.Condition.Create.If(true).FailsWith("Condition should be true").ToProcessOutput();

        Assert.True(output.Success);
    }

    [Fact]
    public void Should_Succeed_When_ThereAreMultipleTrueExpressions()
    {
        var output = Condition.Condition.Create
            .If(true).FailsWith("Condition 1 should be true")
            .IfNot(false).FailsWith("Condition 2 should be false")
            .ToProcessOutput();

        Assert.True(output.Success);
    }

    [Fact]
    public void ShouldNot_Succeed()
    {
        var output = Condition.Condition.Create.If(false).FailsWith("Condition should be true").ToProcessOutput();

        Assert.False(output.Success);
    }

    [Fact]
    public void ShouldNot_Succeed_When_OneExpressionIsFalse()
    {
        var output = Condition.Condition.Create
            .If(true).FailsWith("Condition 1 should be true")
            .IfNot(false).FailsWith("Condition 2 should be false")
            .If(false).FailsWith("Condition 3 should be true")
            .ToProcessOutput();

        Assert.False(output.Success);
        Assert.Equal("Condition 3 should be true", output.Errors.First());
    }
}
