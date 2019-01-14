using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
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
        [HttpGet("byUserid")]
        public IEnumerable<Tasklists> GetTasklists([FromQuery] string userid)
        {
            return _context.Tasklists.Where(tl => tl.User == userid);
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

        [HttpGet]
        [Route("downloadzip")]
        public ActionResult DownloadZip([FromQuery] string userid)
        {
            var tasklists = _context.Tasklists.Where(q => q.User == userid);//todo non empty
            var tasks = _context.Tasks.Where(t => tasklists.Contains(t.Tasklist)).ToList();
            var zipItems = tasklists.Select(t => new ZipItem($"{t.Name}.txt", new MemoryStream(Encoding.UTF8.GetBytes(GetTasklistContent(t, tasks))))).ToList();
            var zipStream = Zipper.Zip(zipItems);
            return File(zipStream, "application/octet-stream", "My todolists.zip");
        }

        private string GetTasklistContent(Tasklists tasklist, List<Tasks> tasks)
        {
            string result = "";
            tasks.Where(t => t.Tasklist == tasklist).ToList().ForEach(t =>
            {
                string iscomplete = t.Iscompleted ? "[Выполнена]" : "[Не выполнена]";
                result += $"{t.Description} {iscomplete}" + Environment.NewLine;
            });
            return result;
        }

        public class ZipItem
        {
            public string Name { get; set; }
            public Stream Content { get; set; }
            public ZipItem(string name, Stream content)
            {
                this.Name = name;
                this.Content = content;
            }
            public ZipItem(string name, string contentStr, Encoding encoding)
            {
                // convert string to stream
                var byteArray = encoding.GetBytes(contentStr);
                var memoryStream = new MemoryStream(byteArray);
                this.Name = name;
                this.Content = memoryStream;
            }
        }

        public static class Zipper
        {
            public static Stream Zip(List<ZipItem> zipItems)
            {
                var zipStream = new MemoryStream();

                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var zipItem in zipItems)
                    {
                        var entry = zip.CreateEntry(zipItem.Name);
                        using (var entryStream = entry.Open())
                        {
                            zipItem.Content.CopyTo(entryStream);
                        }
                    }
                }
                zipStream.Position = 0;
                return zipStream;
            }
        }
    }
}