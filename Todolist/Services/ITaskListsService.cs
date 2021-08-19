using System.Collections.Generic;
using System.Threading.Tasks;
using Todolist.Dtos;
using Todolist.Models;
using Task = System.Threading.Tasks.Task;

namespace Todolist.Services
{
    public interface ITaskListsService
    {
        Task<IEnumerable<TaskListDto>> GetUserTaskLists();
        Task<TaskListDto> GetTaskListByUrl(string url);
        Task<TaskListDto> GetTaskListData(int id);
        Task<TaskList> GetTaskList(int id);
        Task CreateTaskList(AddTaskListDto newTaskList);
        Task DeleteTaskList(int id);
        Task UpdateTaskList(UpdateTaskListDto updatedTaskList);
        Task<string> GetTaskListUrl(int id);
    }
}