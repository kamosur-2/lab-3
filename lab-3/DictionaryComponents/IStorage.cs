namespace lab_2.DictionaryComponents;

public interface IStorage
{ 
    public void AddNewWord(string word, StorageContext storageContext);
    public IList<Word>? WordSearch(string word);
}