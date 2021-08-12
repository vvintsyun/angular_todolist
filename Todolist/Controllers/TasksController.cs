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
        public IActionResult GetTasks([FromRoute] int taskListId)
        {
            var result = _tasksService.GetTaskTasksByTaskList(taskListId);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpGet("byTaskListUrl/{taskListUrl}")]
        [AllowAnonymous]
        public IActionResult GetTasks([FromRoute] string taskListUrl)
        {
            var result = _tasksService.GetTaskListTasksByUrlDto(taskListUrl);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _tasksService.GetTask(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult PutTask([FromRoute] int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateTaskDto.Id)
            {
                return BadRequest();
            }
            
            _tasksService.UpdateTask(updateTaskDto);

            return Ok();
        }
        
        [HttpPut("updateCompleted")]
        [AllowAnonymous]
        public IActionResult UpdateCompleted([FromBody] UpdateTaskCompletedDto updateTaskCompletedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _tasksService.UpdateCompleted(updateTaskCompletedDto);

            return Ok();
        }

        [HttpPost]
        public IActionResult PostTask([FromBody] AddTaskDto addTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _tasksService.CreateTask(addTaskDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _tasksService.DeleteTask(id);
            return Ok();
        }
    }
}