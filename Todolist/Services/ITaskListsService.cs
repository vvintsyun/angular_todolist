using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Todolist.Dtos;
using Todolist.Models;
using Task = System.Threading.Tasks.Task;

namespace Todolist.Services
{
    public interface ITaskListsService
    {
        Task<IEnumerable<TaskListDto>> GetUserTaskLists(CancellationToken ct);
        Task<TaskListDto> GetTaskListByUrl(string url, CancellationToken ct);
        Task<TaskListDto> GetTaskListData(int id, CancellationToken ct);
        Task<TaskList> GetTaskList(int id, CancellationToken ct);
        Task CreateTaskList(AddTaskListDto newTaskList, CancellationToken ct);
        Task DeleteTaskList(int id, CancellationToken ct);
        Task UpdateTaskList(UpdateTaskListDto updatedTaskList, CancellationToken ct);
        Task<string> GetTaskListUrl(int id, CancellationToken ct);
    }
}