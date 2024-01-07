using WebAPIApp.Controllers;

namespace lab_2.DictionaryComponents;

public interface IStorage
{ 
    public void AddNewWord(string word, StorageController controller);
    public IList<Word>? WordSearch(string word);
}