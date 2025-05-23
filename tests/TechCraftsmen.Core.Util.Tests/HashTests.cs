using TechCraftsmen.Core.Util.Hashing;

namespace TechCraftsmen.Core.Util.Tests;

public class HashTests
{
    [Fact]
    public void Should_MatchHash()
    {
        const string text = "HelloWorld";
        
        var hash = Hash.NewFromText(text);
        var matches = hash.TextMatches(text);
        
        Assert.True(matches);
    }
    
    [Fact]
    public void Should_NotMatchHash()
    {
        const string text1 = "HelloWorld";
        const string text2 = "GoodbyeWorld";
        
        var hash = Hash.NewFromText(text1);
        
        var matches = hash.TextMatches(text2);
        
        Assert.False(matches);
    }

    [Fact]
    public void Should_MatchForNewHashFromBytes()
    {
        const string text = "HelloWorld";
        
        var hash = Hash.NewFromText(text);
        var testHash = Hash.NewFromBytes(hash.Value, hash.Salt);
        
        var matches = testHash.TextMatches(text);
        
        Assert.True(matches);
    }
    
    [Fact]
    public void Should_NotMatchForNewHashFromBytes()
    {
        const string text = "HelloWorld";
        const string text2 = "GoodbyeWorld";
        
        var hash = Hash.NewFromText(text);
        var testHash = Hash.NewFromBytes(hash.Value, hash.Salt);
        
        var matches = testHash.TextMatches(text2);
        
        Assert.False(matches);
    }
    
    [Fact]
    public void Should_ProduceDifferentHashesForSameText()
    {
        const string text = "HelloWorld";
    
        var hash1 = Hash.NewFromText(text);
        var hash2 = Hash.NewFromText(text);
        
        Assert.NotEqual(hash1.Salt, hash2.Salt);
        Assert.NotEqual(hash1.Value, hash2.Value);
    }
    
    [Fact]
    public void Should_MatchForSameText()
    {
        const string text = "HelloWorld";
    
        var hash1 = Hash.NewFromText(text);
        var hash2 = Hash.NewFromText(text);
        
        Assert.True(hash1.TextMatches(text));
        Assert.True(hash2.TextMatches(text));
    }
}
