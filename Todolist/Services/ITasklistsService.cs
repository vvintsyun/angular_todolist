using System.Collections.Generic;
using Todolist.Dtos;
using Todolist.Models;

namespace Todolist.Services
{
    public interface ITasklistsService
    {
        List<Tasklist> GetUserTasklists();
        List<TasklistDto> GetUserTasklistsDto();
        Tasklist GetTasklistByUrl(string url);
        Tasklist GetTasklistData(int id);
        Tasklist CreateTasklist(AddTasklistDto newTasklist);
        void DeleteTasklist(Tasklist tasklist);
        void UpdateTasklist(Tasklist tasklist, UpdateTasklistDto updatedTasklist);
        string GetTasklistUrl(int id);
    }
}