using WebAPIApp.Controllers;

namespace lab_2.DictionaryComponents;

public class SingleRootWordsDictionary : IStorage
{
    public SingleRootWordsDictionary(List<List<Word>> storage)
    {
        Storage = storage;
    }

    public List<List<Word>> Storage { get; }

    public IList<Word>? WordSearch(string word)
    {
        foreach (var singleRootCollection in Storage)
        {
            if (singleRootCollection.Any(i => i.fullWord == word))
            {
                return singleRootCollection;
            }
        }

        return null;
    }
    
    public IList<Word>? RootSearch(string root)
    {
        foreach (var singleRootCollection in Storage)
        {
            if (singleRootCollection[0].Root == root)
            {
                return singleRootCollection;
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
                    var list = new List<Word>();
                    list.Add(new Word(prefix, root, postfix, word, JSONDictionary.HashWord(word)));
                    Storage.Add(list);
                    controller.Post(new Word(prefix, root, postfix, word, JSONDictionary.HashWord(word)));
                    Storage[^1].OrderBy(p => p.fullWord);
                    Console.WriteLine();
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

