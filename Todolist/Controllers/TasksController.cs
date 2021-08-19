using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todolist.Dtos;
using Todolist.Services;

namespace Todolist.Controllers
{
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpGet("byTaskListId/{taskListId}")]
        public async Task<IActionResult> GetTasks([FromRoute] int taskListId)
        {
            var result = await _tasksService.GetTaskTasksByTaskList(taskListId);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpGet("byTaskListUrl/{taskListUrl}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTasks([FromRoute] string taskListUrl)
        {
            var result = await _tasksService.GetTaskListTasksByUrlDto(taskListUrl);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _tasksService.GetTask(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask([FromRoute] int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateTaskDto.Id)
            {
                return BadRequest();
            }
            
            await _tasksService.UpdateTask(updateTaskDto);

            return Ok();
        }
        
        [HttpPut("updateCompleted")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCompleted([FromBody] UpdateTaskCompletedDto updateTaskCompletedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await _tasksService.UpdateCompleted(updateTaskCompletedDto);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] AddTaskDto addTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tasksService.CreateTask(addTaskDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tasksService.DeleteTask(id);
            return Ok();
        }
    }
}