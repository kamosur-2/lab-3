using lab_2.DictionaryComponents;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIApp.Controllers;

public interface IController
{
    public Task<ActionResult<IEnumerable<Word>>> Get();
    public Task<ActionResult<Word>> Post(Word? word);

}