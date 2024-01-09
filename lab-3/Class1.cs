using lab_2.DictionaryComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebAPIApp.Controllers;

namespace lab_2;

public class Class1
{
    public Class1(IStorage storage)
    {
        Store = storage;
    }

    public Class1()
    {
        
    }
    
    public IStorage Store { get; }
    public static void Start()
    {
        var wordsDictionary = new SingleRootWordsDictionary(new List<List<Word>>());
        var jsonDictionary = new JSONDictionary(new List<List<string>>());
        var xmlDictionary = new XMLDictionary();
        var storageContext = new StorageContext();
        var storageController = new StorageController(storageContext);
        var word = string.Empty;
        while (word != "q")
        {
            Console.WriteLine("Куда хотите сохранить новое слово?: \n" + "1.XMLDictionary\n" + "2.JSONDictionary\n" 
                               + "3.SingleRootWordsDictionary");
            string num = Console.ReadLine();

            IStorage dictionary = null;

            if (num == "1")
            {
                dictionary = xmlDictionary;
            }
            else if (num == "2")
            {
                dictionary = jsonDictionary;
            }
            else if (num == "3")
            {
                dictionary = wordsDictionary;
                
            }
            
            word = Console.ReadLine();
            if (dictionary is not null && dictionary.WordSearch(word) is null)
            {
                Console.WriteLine("хотите ли внести слово в словарь (y/n)?");
                string ans = Console.ReadLine();
                if (ans == "y")
                {
                    dictionary.AddNewWord(word, storageController);
                }
            }
            else
            {
                var collection = dictionary.WordSearch(word);
                Console.WriteLine("Известные однокоренные слова: ");
                foreach (var i in collection)
                {
                    PrintWord(i);
                }
            }
        }
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
        Console.WriteLine(word.Postfix[^2]);
    }

    public void Add(string word, StorageController storageController)
    {
        Store.AddNewWord(word, storageController);
    }
    
    public void StartForTest(string num, string word)
    {
        
        var storageContext = new StorageContext();
        var storageController = new StorageController(storageContext);
        
        if (Store.WordSearch(word) is null) 
        { 
            Store.AddNewWord(word, storageController);
        }
        else 
        { 
            var collection = Store.WordSearch(word); 
            foreach (var i in collection) 
            { 
                PrintWord(i);
            }
        }
    }
}