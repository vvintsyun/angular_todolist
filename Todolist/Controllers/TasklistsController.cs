using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todolist.Models;

namespace Todolist.Controllers
{
    [Route("api/tasklists")]
    [ApiController]
    public class TasklistsController : ControllerBase
    {
        private readonly Context _context;

        public TasklistsController(Context context)
        {
            _context = context;
        }

        // GET: api/Tasklists
        [HttpGet]
        public IEnumerable<Tasklists> GetTasklists()
        {
            return _context.Tasklists;
        }

        // GET: api/Tasklists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTasklist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasklist = await _context.Tasklists.FindAsync(id);

            if (tasklist == null)
            {
                return NotFound();
            }

            return Ok(tasklist);
        }

        // PUT: api/Tasklists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasklist([FromRoute] int id, [FromBody] Tasklists tasklist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tasklist.Id)
            {
                return BadRequest();
            }

            _context.Entry(tasklist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasklistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasklists
        [HttpPost]
        public async Task<IActionResult> PostTasklist([FromBody] Tasklists tasklist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tasklists.Add(tasklist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTasklist", new { id = tasklist.Id }, tasklist);
        }

        // DELETE: api/Tasklists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTasklist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasklist = await _context.Tasklists.FindAsync(id);
            if (tasklist == null)
            {
                return NotFound();
            }

            _context.Tasklists.Remove(tasklist);
            await _context.SaveChangesAsync();

            return Ok(tasklist);
        }

        private bool TasklistExists(int id)
        {
            return _context.Tasklists.Any(e => e.Id == id);
        }
    }
}