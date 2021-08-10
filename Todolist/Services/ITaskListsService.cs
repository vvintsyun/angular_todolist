using System.Collections.Generic;
using Todolist.Dtos;
using Todolist.Models;
using Task = System.Threading.Tasks.Task;

namespace Todolist.Services
{
    public interface ITaskListsService
    {
        IEnumerable<TaskListDto> GetUserTaskLists();
        TaskListDto GetTaskListByUrl(string url);
        TaskListDto GetTaskListData(int id);
        TaskList GetTaskList(int id);
        Task CreateTaskList(AddTaskListDto newTaskList);
        void DeleteTaskList(int id);
        void UpdateTaskList(UpdateTaskListDto updatedTaskList);
        string GetTaskListUrl(int id);
    }
}