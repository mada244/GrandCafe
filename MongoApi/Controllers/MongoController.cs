using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MongoController<T> : ControllerBase where T : class
    {
        protected readonly MongoRepository<T> _repository;

        public MongoController(string collectionName)
        {
            const string connectionUri = "mongodb+srv://madaciumac:loMI8Qu7WjARKcf2@grandcaffe.msc3lmg.mongodb.net/GrandCaffe?retryWrites=true&w=majority&appName=GrandCaffe";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("GrandCaffe");
            _repository = new MongoRepository<T>(database, collectionName);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(string id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(T item)
        {
            await _repository.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.ToBsonDocument()["_id"].ToString() }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, T item)
        {
            await _repository.UpdateAsync(id, item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool deleted = await _repository.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet("exists")]
        //public async Task<IActionResult> CheckExists(string email)
        //{
        //    if (string.IsNullOrWhiteSpace(email))
        //    {
        //        return BadRequest("Invalid email provided.");
        //    }

        //    bool exists = await _repository.ExistsAsync(email);  // Implement this method in your service
        //    return Ok(exists);
        //}

    }
}

