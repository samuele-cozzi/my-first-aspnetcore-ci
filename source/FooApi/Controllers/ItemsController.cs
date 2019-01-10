using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FooApi.Models;
using FooApi.Repositories;
using FooApi.Core;
using Newtonsoft.Json;


namespace FooApi.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private IDocumentDBRepository<Item> repo;
        private readonly ILogger logger;
        

        public ItemsController(IDocumentDBRepository<Item> _repo, ILogger<ItemsController> _logger){
            this.repo = _repo;
            this.logger = _logger;
        }

        // GET api/items
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return new List<Item>() {
                new Item() { Name="", Description="", Completed=false},
                new Item() { Name="", Description="", Completed=false}
            };
        }

        // GET api/items/5
        [HttpGet("{id}")]
        public Task<Item> Get(string id)
        {
            return repo.GetItemAsync(id);
        }

        // POST api/items
        [HttpPost]
        public async Task<string> Post([FromBody] Item value)
        {
            var json = JsonConvert.SerializeObject(value);
            logger.LogInformation(LoggingEvents.INSERT_ITEM, "json: {0}", json);

            string id = await repo.CreateItemAsync(value);
            return id;
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public string Put(string id, [FromBody] Item value)
        {
            value.id = id;
            var json = JsonConvert.SerializeObject(value);
            logger.LogInformation(LoggingEvents.UPDATE_ITEM, "json: {0}", json);

            repo.UpdateItemAsync(value);
            return id;
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            repo.DeleteItemAsync(id);
        }
    }
}
