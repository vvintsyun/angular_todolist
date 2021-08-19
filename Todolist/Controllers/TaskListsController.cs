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
        public async Task<IActionResult> GetTaskLists()
        {
            var result = await _taskListsService.GetUserTaskLists();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("TaskListByUrl/{url}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTaskListIdByUrl([FromRoute] string url)
        {
            var taskList = await _taskListsService.GetTaskListByUrl(url);

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
        public async Task<IActionResult> GetTaskList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskList = await _taskListsService.GetTaskListData(id);

            if (taskList == null)
            {
                return NotFound();
            }
            
            return Ok(taskList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskList([FromRoute] int id, [FromBody] UpdateTaskListDto updateTaskListDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateTaskListDto.Id)
            {
                return BadRequest();
            }
            
            await _taskListsService.UpdateTaskList(updateTaskListDto);

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
        public async Task<IActionResult> DeleteTaskList(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskListsService.DeleteTaskList(id);
            return Ok();
        }

        [HttpGet("geturl/{taskListId}")]
        public async Task<IActionResult> GetUrl(int taskListId)
        {
            var result = await _taskListsService.GetTaskListUrl(taskListId);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(new {url = result});
        }

        [HttpGet("downloadzip")]
        public async Task<ActionResult> DownloadZip()
        {
            var taskLists = await _taskListsService.GetUserTaskLists();
            if (taskLists == null)
            {
                return NotFound();
            }
            var zipItems = new List<ZipItem>();
            var stringBuilder = new StringBuilder();

            foreach (var taskListDto in taskLists)
            {
                var text = GetTaskListContent(taskListDto, stringBuilder);
                var content = new MemoryStream(Encoding.UTF8.GetBytes(text));
                var item = new ZipItem($"{taskListDto.Name}.txt", content);
                zipItems.Add(item);
            }
            
            var zipStream = Zipper.Zip(zipItems);
            return File(zipStream, "application/octet-stream", "My todolists.zip");
        }

        private string GetTaskListContent(TaskListDto taskList, StringBuilder stringBuilder)
        {
            stringBuilder.Clear();
            var tasks = taskList
                .Tasks
                .ToArray();
            
            foreach (var task in tasks)
            {
                var isCompleted = task.IsCompleted ? "[Completed]" : "[Not Completed]";
                stringBuilder.Append($"{task.Description} {isCompleted}" + Environment.NewLine);
            }
            return stringBuilder.ToString();
        }
    }
}