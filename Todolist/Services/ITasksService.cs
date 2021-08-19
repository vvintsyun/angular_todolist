using System.Collections.Generic;
using System.Threading.Tasks;
using Todolist.Dtos;

namespace Todolist.Services
{
    public interface ITasksService
    {
        Task<List<TaskDto>> GetTaskTasksByTaskList(int taskListId);
        Task<List<TaskDto>> GetTaskListTasksByUrlDto(string taskListUrl);
        Task<Todolist.Models.Task> GetTask(int id, bool allowAnonymous = false);
        Task CreateTask(AddTaskDto newTask);
        Task UpdateTask(UpdateTaskDto updatedTask);
        Task UpdateCompleted(UpdateTaskCompletedDto updatedCompleted);
        Task DeleteTask(int id);
    }
}