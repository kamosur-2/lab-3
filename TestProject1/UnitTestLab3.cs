using Xunit;
using NSubstitute;
using System.Text.Json;
using lab_2;
using lab_2.DictionaryComponents;
using WebAPIApp.Controllers;

namespace TestProject1;

public class UnitTestLab3
{
    [Fact]
    public static void TestWordSearch()
    {
        var mock = Substitute.For<IStorage>();
        var class1 = new Class1(mock);
        class1.StartForTest("2", "sleep");
        Assert.NotEmpty(mock.ReceivedCalls().Where(x => x.GetMethodInfo().Name == "WordSearch"));
    }

    [Fact]
    public static void TestAddNewWord()
    {
        var mock = Substitute.For<IStorage>();
        var class1 = new Class1(mock);
        class1.Add("sleep", new StorageController(new StorageContext()));
        Assert.Single(mock.ReceivedCalls().Where(x => x.GetMethodInfo().Name == "AddNewWord"));
    }
    
    [Fact]
    public static void TestRootSearchNull()
    {
        var words = new List<Word>();
        words.Add(new Word(new List<string>(), "сон", new List<string>(), "сон", JSONDictionary.HashWord("сон")));
        var storage = new List<List<Word>>();
        storage.Add(words);
        var dictionary = new SingleRootWordsDictionary(storage);
        Assert.Null(dictionary.RootSearch("мечт"));
    }
    
    [Fact]
    public static void TestWordSearchNull()
    {
        var words = new List<Word>();
        words.Add(new Word(new List<string>(), "сон", new List<string>(), "сон", JSONDictionary.HashWord("сон")));
        var storage = new List<List<Word>>();
        storage.Add(words);
        var dictionary = new SingleRootWordsDictionary(storage);
        Assert.Null(dictionary.WordSearch("мечта"));
    }
    
    [Fact]
    public static void TestWordSearchNotNull()
    {
        var words = new List<Word>();
        words.Add(new Word(new List<string>(), "сон", new List<string>(), "сон", JSONDictionary.HashWord("сон")));
        var storage = new List<List<Word>>();
        storage.Add(words);
        var dictionary = new SingleRootWordsDictionary(storage);
        Assert.NotNull(dictionary.WordSearch("сон"));
        Assert.Equal(words,dictionary.WordSearch("сон"));
    }
}