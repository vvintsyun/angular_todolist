using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Todolist.Dtos;
using Todolist.Models;
using Todolist.Services;

namespace Todolist.Controllers
{
    [Route("api/tasklists")]
    [ApiController]
    public class TasklistsController : ControllerBase
    {
        private readonly ITasklistsService _tasklistsService;

        public TasklistsController(ITasklistsService tasklistsService)
        {
            _tasklistsService = tasklistsService;
        }

        [HttpGet]
        public IActionResult GetTasklists()
        {
            var result = _tasklistsService.GetUserTasklistsDto();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("TasklistByUrl/{url}")]
        public IActionResult GetTasklistidByUrl(string url)
        {
            var tasklist = _tasklistsService.GetTasklistByUrl(url);

            if (tasklist == null)
            {
                return NotFound();
            }

            var result = new TasklistDto
            {
                Id = tasklist.Id,
                Name = tasklist.Name
            };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetTasklist(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasklist = _tasklistsService.GetTasklistData(id);

            if (tasklist == null)
            {
                return NotFound();
            }

            var tasklistDto = new TasklistDto
            {
                Id = tasklist.Id,
                Name = tasklist.Name
            };
            return Ok(tasklistDto);
        }

        [HttpPut("{id}")]
        public IActionResult PutTasklist([FromRoute] int id, [FromBody] UpdateTasklistDto updateTasklistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateTasklistDto.Id)
            {
                return BadRequest();
            }

            var tasklist = _tasklistsService.GetTasklistData(id);
            _tasklistsService.UpdateTasklist(tasklist, updateTasklistDto);

            return Ok();
        }

        [HttpPost]
        public IActionResult PostTasklist([FromBody] AddTasklistDto addTasklistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasklist = _tasklistsService.CreateTasklist(addTasklistDto);

            return CreatedAtAction("GetTasklist", new { id = tasklist.Id }, tasklist);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTasklist(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasklist = _tasklistsService.GetTasklistData(id);
            if (tasklist == null)
            {
                return NotFound();
            }

            _tasklistsService.DeleteTasklist(tasklist);
            return Ok();
        }

        [HttpGet("geturl/{tasklistid}")]
        public IActionResult GetUrl(int tasklistid)
        {
            var result = _tasklistsService.GetTasklistUrl(tasklistid);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(new {url = result});
        }

        [HttpGet("downloadzip")]
        public ActionResult DownloadZip()
        {
            var tasklists = _tasklistsService.GetUserTasklists();
            if (tasklists == null)
            {
                return NotFound();
            }
            List<ZipItem> zipItems = new List<ZipItem>();
            tasklists.ForEach(tl =>
            {
                var text = GetTasklistContent(tl);
                var content = new MemoryStream(Encoding.UTF8.GetBytes(text));
                var item = new ZipItem($"{tl.Name}.txt", content);
                zipItems.Add(item);
            });
            
            var zipStream = Zipper.Zip(zipItems);
            return File(zipStream, "application/octet-stream", "My todolists.zip");
        }

        private string GetTasklistContent(Tasklist tasklist)
        {
            string result = "";
            var tasks = tasklist
                .Tasks
                .ToArray();
            foreach (var task in tasks)
            {
                string isCompleted = task.Iscompleted ? "[Completed]" : "[Not Completed]";
                result += $"{task.Description} {isCompleted}" + Environment.NewLine;
            }
            return result;
        }
    }
}