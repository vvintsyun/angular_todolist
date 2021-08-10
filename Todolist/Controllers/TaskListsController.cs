using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todolist.Dtos;
using Todolist.Models;
using Todolist.Services;

namespace Todolist.Controllers
{
    [Route("api/tasklists")]
    [ApiController]
    [Authorize]
    public class TaskListsController : ControllerBase
    {
        private readonly ITaskListsService _taskListsService;

        public TaskListsController(ITaskListsService taskListsService)
        {
            _taskListsService = taskListsService;
        }

        [HttpGet]
        public IActionResult GetTaskLists()
        {
            var result = _taskListsService.GetUserTaskLists();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("TaskListByUrl/{url}")]
        public IActionResult GetTaskListIdByUrl(string url)
        {
            var taskList = _taskListsService.GetTaskListByUrl(url);

            if (taskList == null)
            {
                return NotFound();
            }

            var result = new TaskListDto
            {
                Id = taskList.Id,
                Name = taskList.Name
            };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskList(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskList = _taskListsService.GetTaskListData(id);

            if (taskList == null)
            {
                return NotFound();
            }
            
            return Ok(taskList);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTaskList([FromRoute] int id, [FromBody] UpdateTaskListDto updateTaskListDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateTaskListDto.Id)
            {
                return BadRequest();
            }
            
            _taskListsService.UpdateTaskList(updateTaskListDto);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskList([FromBody] AddTaskListDto addTaskListDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskListsService.CreateTaskList(addTaskListDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTaskList(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _taskListsService.DeleteTaskList(id);
            return Ok();
        }

        [HttpGet("geturl/{taskListId}")]
        public IActionResult GetUrl(int taskListId)
        {
            var result = _taskListsService.GetTaskListUrl(taskListId);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(new {url = result});
        }

        [HttpGet("downloadzip")]
        public ActionResult DownloadZip()
        {
            var taskLists = _taskListsService.GetUserTaskLists();
            if (taskLists == null)
            {
                return NotFound();
            }
            List<ZipItem> zipItems = new List<ZipItem>();
            
            foreach (var taskListDto in taskLists)
            {
                var text = GetTaskListContent(taskListDto);
                var content = new MemoryStream(Encoding.UTF8.GetBytes(text));
                var item = new ZipItem($"{taskListDto.Name}.txt", content);
                zipItems.Add(item);
            }
            
            var zipStream = Zipper.Zip(zipItems);
            return File(zipStream, "application/octet-stream", "My todolists.zip");
        }

        private string GetTaskListContent(TaskListDto taskList)
        {
            string result = "";
            var tasks = taskList
                .Tasks
                .ToArray();
            //rewrite mapping
            foreach (var task in tasks)
            {
                string isCompleted = task.IsCompleted ? "[Completed]" : "[Not Completed]";
                result += $"{task.Description} {isCompleted}" + Environment.NewLine;
            }
            return result;
        }
    }
}