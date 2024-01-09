using System.Xml.Serialization;
using WebAPIApp.Controllers;

namespace lab_2.DictionaryComponents;

public class XMLDictionary : IStorage
{
    public List<List<string>> Storage { get; } = new List<List<string>>();

    public IList<Word>? WordSearch(string word)
    {
        var mySerializer = new XmlSerializer(typeof(Word));
        foreach (var singleRootCollection in Storage)
        {
            if (singleRootCollection.Any(i =>  ((Word)mySerializer.Deserialize(new FileStream(i, FileMode.Open))).fullWord == word))
            {
                List<Word> words = new List<Word>();
                foreach (var i in singleRootCollection)
                {
                    words.Add((Word)mySerializer.Deserialize(new FileStream(i, FileMode.Open)));
                }

                return words;
            }
        }

        return null;
    }
    
    public IList<Word>? RootSearch(string root)
    {
        var mySerializer = new XmlSerializer(typeof(Word));
        foreach (var singleRootCollection in Storage)
        {
            if (((Word)mySerializer.Deserialize(new FileStream(singleRootCollection[0], FileMode.Open))).Root == root)
            {
                List<Word> words = new List<Word>();
                foreach (var i in singleRootCollection)
                {
                    words.Add((Word)mySerializer.Deserialize(new FileStream(i, FileMode.Open)));
                }

                return words;
            }
        }

        return null;
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
                    XmlSerializer mySerializer = new XmlSerializer(typeof(Word)); 
                    StreamWriter myWriter = new StreamWriter($"{word}.xml");
                    mySerializer.Serialize(myWriter, new Word(prefix, root, postfix, word, JSONDictionary.HashWord(word)));
                    list.Add($"{word}.xml");
                    Storage.Add(list);
                    controller.Post(new Word(prefix, root, postfix, word, JSONDictionary.HashWord(word)));
                    using var myFileStream = new FileStream($"{word}.xml", FileMode.Open);
                    Storage[^1].OrderBy(p => ((Word)mySerializer.Deserialize(myFileStream)).fullWord);
                }
                else
                {
                    RootSearch(root).Add(new Word(prefix, root, postfix, word, JSONDictionary.HashWord(word)));
                }
                Console.Write("Слово ");
                PrintWord(new Word(prefix, root, postfix, word, JSONDictionary.HashWord(word)));
                Console.WriteLine(" добавлено");
                var f = controller.Get();
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
    
    public static void PrintWord(Word word)
    {
        for (int j = 0; j < word.Prefix.Count - 1; j++)
        {

            Console.Write(word.Prefix[j] + "-");

        }
        
        Console.Write(word.Root + "-");
        for (int j = 0; j < word.Postfix.Count - 2; j++)
        {

            Console.Write(word.Postfix[j] + "-");

        }
        Console.Write(word.Postfix[^2]);
    }
}