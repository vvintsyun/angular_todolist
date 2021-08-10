using System.Collections.Generic;
using Todolist.Dtos;
using Todolist.Models;

namespace Todolist.Services
{
    public interface ITasksService
    {
        List<TaskDto> GetTaskTasksByTaskList(int taskListId);
        List<TaskDto> GetTaskListTasksByUrlDto(string taskListUrl);
        Task GetTask(int id);
        void CreateTask(AddTaskDto newTask);
        void UpdateTask(UpdateTaskDto updatedTask);
        void DeleteTask(int id);
    }
}