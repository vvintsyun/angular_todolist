using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Todolist.Dtos;

namespace Todolist.Services
{
    public interface ITasksService
    {
        Task<List<TaskDto>> GetTaskTasksByTaskList(int taskListId, CancellationToken ct);
        Task<List<TaskDto>> GetTaskListTasksByUrlDto(string taskListUrl, CancellationToken ct);
        Task<Todolist.Models.Task> GetTask(int id, CancellationToken ct, bool allowAnonymous = false);
        Task CreateTask(AddTaskDto newTask, CancellationToken ct);
        Task UpdateTask(UpdateTaskDto updatedTask, CancellationToken ct);
        Task UpdateCompleted(UpdateTaskCompletedDto updatedCompleted, CancellationToken ct);
        Task DeleteTask(int id, CancellationToken ct);
    }
}