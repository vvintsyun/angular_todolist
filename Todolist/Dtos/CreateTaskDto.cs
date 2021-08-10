using Todolist.Models;

namespace Todolist.Dtos
{
    public class CreateTaskDto
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public TaskList TaskList { get; set; }
    }
}