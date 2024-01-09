using lab_2.DictionaryComponents;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using NSubstitute;
using WebAPIApp.Controllers;

namespace TestProject1;

public class UnitTestLab4
{
    [Fact]
    public static void TestControllerGetRequest()
    {
        var mock = Substitute.For<IController>();
        var dict = new JSONDictionary(new List<List<string>>());
        dict.Get(mock);
        Assert.Single(mock.ReceivedCalls().Where(x => x.GetMethodInfo().Name == "Get"));
    }
    
    [Fact]
    public static void TestControllerPostRequest()
    {
        var mock = Substitute.For<IController>();
        var dict = new JSONDictionary(new List<List<string>>());
        dict.Post(mock, new Word(new List<string>(), "сон", new List<string>(), "сон", JSONDictionary.HashWord("сон")));
        Assert.Single(mock.ReceivedCalls().Where(x => x.GetMethodInfo().Name == "Post"));
    }

    [Theory]
    [InlineData ("abc")]
    public static void TestHashWord(string test)
    {
        Assert.Equal(38666830, JSONDictionary.HashWord(test));
    }
    
    [Theory]
    [InlineData ("ab")]
    public static void TestHashWordNotEqual(string test)
    {
        Assert.NotEqual(38666830, JSONDictionary.HashWord(test));
    }
}