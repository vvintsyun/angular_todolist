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

        // GET: api/Tasklists
        [HttpGet("TasklistIdbyUrl")]
        public int GetTasklistidByUrl([FromQuery] string url)
        {
            return _context.Tasklists.First(tl => tl.Url == url).Id;
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

        [Route("geturl")]
        public string GetUrl([FromQuery] int tasklistid)
        {
            var tasklist = _context.Tasklists.First(tl => tl.Id == tasklistid);
            var busyUrls = _context.Tasklists.Select(tl => tl.Url).ToList();
            if (string.IsNullOrEmpty(tasklist.Url))
            {
                string newUrl;
                do
                {
                    newUrl = RandomURL.GetURL();
                }
                while (busyUrls.Contains(newUrl));
                tasklist.Url = newUrl;
                _context.SaveChanges();
            }
            return tasklist.Url;
        }

        [HttpGet]
        [Route("downloadzip")]
        public ActionResult DownloadZip([FromQuery] string userid)
        {
            var tasklists = _context.Tasklists.Where(q => q.User == userid).ToList();//todo non empty
            var tasks = _context.Tasks.Where(t => tasklists.Contains(t.Tasklist)).ToList();
            List<ZipItem> zipItems = new List<ZipItem>();
            tasklists.ForEach(tl => 
            {
                var content = new MemoryStream(Encoding.UTF8.GetBytes(GetTasklistContent(tl, tasks)));
                var item = new ZipItem($"{tl.Name}.txt", content);
                zipItems.Add(item);
            });
            
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

        public static class RandomURL
        {
            private static List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            private static List<char> characters = new List<char>()
            {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B',
            'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S',  'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_'};

            public static string GetURL()
            {
                string URL = "";
                Random rand = new Random();
                for (int i = 0; i <= 10; i++)
                { 
                    int random = rand.Next(0, 3);
                    if (random == 1) //add number
                    {                          
                        random = rand.Next(0, numbers.Count);
                        URL += numbers[random].ToString();
                    }
                    else
                    {
                        random = rand.Next(0, characters.Count);
                        URL += characters[random].ToString();
                    }
                }
                return URL;
            }
        }
    }
}