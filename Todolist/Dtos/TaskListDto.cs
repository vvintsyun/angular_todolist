using System.Collections.Generic;

namespace Todolist.Dtos
{
    public class TaskListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskDto> Tasks { get; set; }
    }
}