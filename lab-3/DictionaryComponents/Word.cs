namespace lab_2.DictionaryComponents;

public record Word
{
    public Word(IList<string> prefix, string root, IList<string> postfix, string fullWord, int id)
    {
        this.Prefix = prefix;
        Root = root;
        this.Postfix = postfix;
        this.fullWord = fullWord;
        WordId = id;
    }

    public Word()
    {
        
    }
    
    public int WordId { get; set; }
    public IList<string> Prefix { get; } 
    public string Root { get; }
    public IList<string> Postfix { get; }
    
    public string fullWord { get; }

    public void Print()
    {
        Console.WriteLine(this.fullWord);
    }
}