using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using lab_2.DictionaryComponents;


namespace WebAPIApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase, IController
    {
        public StorageController(StorageContext context)
        {
            Db = context;
            if (!Db.JsonDbSet.Any())
            {
                Db.JsonDbSet.Add(new Word(new List<string>(), "ночь", new List<string>(), "ночь", 233));
                Db.JsonDbSet.Add(new Word(new List<string>(), "сон", new List<string>(), "ночь", 233));
                Db.SaveChanges();
            }
        }
        public StorageContext Db { get; private set; }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Word>>> Get()
        {
            return await Db.JsonDbSet.ToListAsync();
        }
 
        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Word>> Get(int id)
        {
            Word word = await Db.JsonDbSet.FirstOrDefaultAsync(x => x.WordId == id);
            if (word == null)
                return NotFound();
            return new ObjectResult(word);
        }
 
        // POST api/users
        [HttpPost]
        public async Task<ActionResult<Word>> Post(Word? word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            if (word is not null)
            {
                Db.AddNewWord(word.fullWord);
            }

            await Db.SaveChangesAsync();
            return Ok(word);
        }
 
        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<Word>> Put(Word? word)
        {
            if (word == null)
            {
                return BadRequest();
            }
            if (!Db.JsonDbSet.Any(x => x.WordId ==word.WordId))
            {
                return NotFound();
            }
 
            Db.Update(word);
            await Db.SaveChangesAsync();
            return Ok(word);
        }
 
        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Word>> Delete(int id)
        {
            Word user = Db.JsonDbSet.FirstOrDefault(x => x.WordId == id);
            if (user == null)
            {
                return NotFound();
            }
            Db.JsonDbSet.Remove(user);
            await Db.SaveChangesAsync();
            return Ok(user);
        }
    }
}