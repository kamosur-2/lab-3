using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebAPIApp.Controllers;

namespace lab_2.DictionaryComponents;

public class JSONDictionary : IStorage
{
    public List<List<string>> Storage { get; } = new List<List<string>>();

    public JSONDictionary(List<List<string>> storage)
    {
        Storage = storage;
    }
    
    public IList<Word>? WordSearch(string word)
    {
        foreach (var singleRootCollection in Storage)
        {
            var t = new Word(new List<string>(), " ", new List<string>(), JsonSerializer.Deserialize<Word>(singleRootCollection[0])?.fullWord, 33);
            if (singleRootCollection.Any(i => JsonSerializer.Deserialize<Word>(i).fullWord == word))
            {
                List<Word> words = new List<Word>();
                foreach (var element in singleRootCollection)
                {
                    words.Add(JsonSerializer.Deserialize<Word>(singleRootCollection[0]));
                }

                return words;
            }
        }

        return null;
    }
    
    public IList<Word>? RootSearch(string root)
    {
        foreach (var singleRootCollection in Storage)
        {
            if (JsonSerializer.Deserialize<Word>(singleRootCollection[0]).Root == root)
            {
                List<Word> words = new List<Word>();
                foreach (var element in singleRootCollection)
                {
                    words.Add(JsonSerializer.Deserialize<Word>(singleRootCollection[0]));
                }

                return words;
            }
        }

        return null;
    }
    
    private List<string> PostPrefBuild(string part)
    {
        var wordPart = new List<string>();
        var newWordPart = " ";
        while (newWordPart != string.Empty)
        {
            Console.WriteLine(part);
            newWordPart = Console.ReadLine();
            wordPart.Add(newWordPart);
        }

        return wordPart;
    }

    public static int HashWord(string word)
    {
        int result = 0;
        for (int i = 0; i < word.Length; i++)
        {
            result += word[i].GetHashCode() * (i + 1);
        }

        return result;
    }

    public void AddNewWord(string word, IController controller)
    {
        var prefix = PostPrefBuild("приставка: ");
        Console.WriteLine("корень");
        var root = Console.ReadLine();
        var postfix = PostPrefBuild("суффикс или окончание: ");

        var checkWord = string.Empty;
        foreach (var i in prefix)
        {
            checkWord += i;
        }

        checkWord += root;
        
        foreach (var i in postfix)
        {
            checkWord += i;
        }

        if (checkWord == word)
        {
            if (root is not null)
            {
                if (RootSearch(root) is null)
                {
                    var list = new List<string>();
                    int number;

                    var wordToAdd = new Word(prefix, root, postfix, word, HashWord(word));
                    list.Add(JsonSerializer.Serialize(wordToAdd));
                    Post(controller, new Word(prefix, root, postfix, word, HashWord(word)));
                    Storage.Add(list);
                    Storage[^1].OrderBy(p => JsonSerializer.Deserialize<Word>(p).fullWord);
                }
                else
                {
                    RootSearch(root).Add(new Word(prefix, root, postfix, word, HashWord(word)));
                }
                Console.Write("Слово ");
                //SingleRootWordsDictionary.PrintWord(new Word(prefix, root, postfix, word, HashWord(word)));
                Console.WriteLine(" добавлено");
                var f = Get(controller);
                List<Word> l = (List<Word>)f.Result.Value;
                Console.WriteLine("Результат Get() запроса: ");
                foreach (var VARIABLE in l)
                {
                    Console.WriteLine(VARIABLE.fullWord);
                }
            }
            else
            {
                Console.WriteLine("слово по частям не соответсвует изначально введенному слову");
            }
        }
    }

    public Task<ActionResult<IEnumerable<Word>>> Get(IController controller)
    {
        return controller.Get();
    }

    public Task<ActionResult<Word>> Post(IController controller, Word word)
    {
        return controller.Post(word);
    }
}