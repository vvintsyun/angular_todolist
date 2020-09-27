using Microsoft.AspNetCore.Mvc;
using Todolist.Dtos;
using Todolist.Services;

namespace Todolist.Controllers
{
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpGet("byTasklistid")]
        public IActionResult GetTasks([FromQuery] int tasklistid)
        {
            var result = _tasksService.GetTasklistTasksDto(tasklistid);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpGet("byTasklistUrl")]
        public IActionResult GetTasks([FromQuery] string tasklistUrl)
        {
            var result = _tasksService.GetTasklistTasksByUrlDto(tasklistUrl);

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

            var task = _tasksService.GetUserTaskData(id);

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

            var task = _tasksService.GetTaskData(id);
            _tasksService.UpdateTask(task, updateTaskDto);

            return Ok();
        }

        [HttpPost]
        public IActionResult PostTask([FromBody] AddTaskDto addTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _tasksService.CreateTask(addTaskDto);

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _tasksService.GetUserTaskData(id);
            if (task == null)
            {
                return NotFound();
            }

            _tasksService.DeleteTask(task);
            return Ok();
        }
    }
}