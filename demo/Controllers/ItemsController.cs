#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using demo.Context;
using demo.Models;
using demo.Dto.Items;
using demo.Extensions.Items;

namespace demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : Controller
    {
        private AppDbContext _context;

        public ItemsController(AppDbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> getItems()
        {
            var items = (await _context.items.ToListAsync())
                .Select(item => item.asDto())
            ;

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> getItem(Guid id)
        {
            Item? item = await _context.items.FindAsync(id);

            if (item == null)
            {
                return NotFound("Item not found");
            }

            return item.asDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> createItem([FromBody] CreateItemDto item)
        {
            Item newItem = new Item
            {
                name = item.name,
                price = item.price
            };

            _context.items.Add(newItem);
            _context.SaveChanges();

            return CreatedAtAction(nameof(getItem), new { id = newItem.id }, newItem.asDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> updateItem(Guid id, UpdateItemDto item)
        {
            Item extistingItem = await _context.items.FindAsync(id);

            if (extistingItem == null)
            {
                return NotFound("Item not found.");
            }

            _context.Entry<Item>(extistingItem).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteItem(Guid id)
        {
            Item existingItem = await _context.items.FindAsync(id);

            if (existingItem == null)
            {
                return NotFound("Item not found");
            }

            _context.items.Remove(existingItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}