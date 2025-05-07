namespace TechCraftsmen.Core.Validation.Tests;

public class RegexCollectionTests
{
    public static TheoryData<string> InvalidEmails => new()
    {
        "-------",
        "@majjf.com",
        "A@b@c@example.com",
        "Abc.example.com",
        "js@proseware..com",
        "ma@@jjf.com",
        "ma@jjf.",
        "ma@jjf..com",
        "ma@jjf.c",
        "ma_@jjf",
        "ma_@jjf.",
        "j@proseware.com9",
        "js@proseware.com9",
        "ma@hostname.comcom",
        "MA@hostname.coMCom"
    };

    public static TheoryData<string> ValidEmails => new()
    {
        "ma_@jjf.com",
        "12@hostname.com",
        "d.j@server1.proseware.com",
        "david.jones@proseware.com",
        "j.s@server1.proseware.com",
        "jones@ms1.proseware.com",
        "m.a@hostname.co",
        "m_a1a@hostname.com",
        "ma.h.saraf.onemore@hostname.com.edu",
        "ma@hostname.com",
        "ma12@hostname.com",
        "ma-a.aa@hostname.com.edu",
        "ma-a@hostname.com",
        "ma-a@hostname.com.edu",
        "ma-a@1hostname.com",
        "ma.a@1hostname.com",
        "ma@1hostname.com"
    };
    
    [Theory]
    [MemberData(nameof(InvalidEmails))]
    public void Should_NotMatch_InvalidEmails(string email)
    {
        var result = RegexCollection.Email().IsMatch(email);

        Assert.False(result);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Should_NotMatch_EmptyOrWhiteSpaceEmails(string email)
    {
        var result = RegexCollection.Email().IsMatch(email);

        Assert.False(result);
    }
    
    [Theory]
    [MemberData(nameof(ValidEmails))]
    public void Should_Match_ValidEmails(string email)
    {
        var result = RegexCollection.Email().IsMatch(email);

        Assert.True(result);
    }
    
    [Fact]
    public void Should_NotMatch_StringWithNoNumber()
    {
        var result = RegexCollection.HasNumber().IsMatch("ABC");
        
        Assert.False(result);
    }

    [Fact]
    public void Should_Match_StringWithNumber()
    {
        var result = RegexCollection.HasNumber().IsMatch("ABC123");
        
        Assert.True(result);
    }
    
    [Fact]
    public void Should_NotMatch_StringWithNoLowerChar()
    {
        var result = RegexCollection.HasLowerChar().IsMatch("ABC");
        
        Assert.False(result);
    }
    
    [Fact]
    public void Should_Match_StringWithLowerChar()
    {
        var result = RegexCollection.HasLowerChar().IsMatch("ABCabc");
        
        Assert.True(result);
    }
    
    [Fact]
    public void Should_NotMatch_StringWithNoUpperChar()
    {
        var result = RegexCollection.HasUpperChar().IsMatch("abc");
        
        Assert.False(result);
    }
    
    [Fact]
    public void Should_Match_StringWithUpperChar()
    {
        var result = RegexCollection.HasUpperChar().IsMatch("abcABC");
        
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("abc")]
    [InlineData("ABC")]
    [InlineData("ABCabc")]
    [InlineData("abc123")]
    [InlineData("ABC123")]
    public void Should_NotMatch_StringWithNoNumberLowerAndUpperChar(string @string)
    {
        var result = RegexCollection.HasNumberLowerAndUpperChar().IsMatch(@string);
        
        Assert.False(result);
    }
    
    [Fact]
    public void Should_Match_StringWithNumberLowerAndUpperChar()
    {
        var result = RegexCollection.HasNumberLowerAndUpperChar().IsMatch("abcABC123");
        
        Assert.True(result);
    }
}