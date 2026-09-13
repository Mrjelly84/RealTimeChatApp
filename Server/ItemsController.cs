using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        // In-memory storage - replace with database in production
        private static List<Item> items = new()
        {
            new Item { Id = 1, Name = "Item 1", Description = "First item" },
            new Item { Id = 2, Name = "Item 2", Description = "Second item" }
        };
        private static int nextId = 3;

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Item item)
        {
            if (item == null)
                return BadRequest();

            item.Id = nextId++;
            items.Add(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, Item updatedItem)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();
            item.Name = updatedItem.Name;
            item.Description = updatedItem.Description;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();
            items.Remove(item);
            return NoContent();
        }
    }
}

