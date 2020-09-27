using System.Collections.Generic;
using Todolist.Dtos;
using Todolist.Models;

namespace Todolist.Services
{
    public interface ITasksService
    {
        List<TaskDto> GetTasklistTasksDto(int tasklistId);
        List<TaskDto> GetTasklistTasksByUrlDto(string tasklistUrl);
        Task GetUserTaskData(int id);
        Task GetTaskData(int id);
        Task CreateTask(AddTaskDto newTask);
        void UpdateTask(Task task, UpdateTaskDto updatedTask);
        void DeleteTask(Task task);
    }
}