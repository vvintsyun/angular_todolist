using System.Threading;
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
        public async Task<IActionResult> GetTasks([FromRoute] int taskListId, CancellationToken ct)
        {
            var result = await _tasksService.GetTaskTasksByTaskList(taskListId, ct);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpGet("byTaskListUrl/{taskListUrl}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTasks([FromRoute] string taskListUrl, CancellationToken ct)
        {
            var result = await _tasksService.GetTaskListTasksByUrlDto(taskListUrl, ct);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] int id, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _tasksService.GetTask(id, ct);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask([FromRoute] int id, [FromBody] UpdateTaskDto updateTaskDto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateTaskDto.Id)
            {
                return BadRequest();
            }
            
            await _tasksService.UpdateTask(updateTaskDto, ct);

            return Ok();
        }
        
        [HttpPut("updateCompleted")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCompleted([FromBody] UpdateTaskCompletedDto updateTaskCompletedDto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await _tasksService.UpdateCompleted(updateTaskCompletedDto, ct);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] AddTaskDto addTaskDto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tasksService.CreateTask(addTaskDto, ct);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tasksService.DeleteTask(id, ct);
            return Ok();
        }
    }
}