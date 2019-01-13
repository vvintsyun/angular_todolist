using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todolist.Models;
//using Task = Todolist.Models.Task;

namespace Todolist.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        //[HttpGet]
        //[Route("/api/google-login")]
        //public async Task LoginGoogle()
        //{
        //    await HttpContext.ChallengeAsync("Google", new AuthenticationProperties() { RedirectUri = "/signin-google" });
        //}

        private readonly Context _context;

        public SampleDataController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/api/downloadzip")]
        public IActionResult DownloadZip()
        {
            var tasklists = _context.Tasklists.Where(q => true/*user*/);//todo non empty
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
