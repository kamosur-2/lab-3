using Microsoft.EntityFrameworkCore;
namespace lab_2.DictionaryComponents;

public class StorageContext : DbContext
{
    public DbSet<Word> JsonDbSet { get; set; }
    public string DbPath { get; }

    public StorageContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "blogging.db");
        Console.WriteLine("Iiiiiiiii:   ");
        Console.WriteLine(DbPath);
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    public void AddNewWord(string word)
    {
        Console.WriteLine("Повторите слово, для внесения в базу данных: ");
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
                var w = new Word(prefix, root, postfix, word, JSONDictionary.HashWord(word));
                this.Add(w);
                this.SaveChanges();
                Console.Write("Слово ");
                PrintWord(w);
                Console.WriteLine(" добавлено в базу данных");
            }
        }
        else
        {
            Console.WriteLine(
                "слово по частям не соответсвует изначально введенному слову и не может быть добавлено в базу данных");
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