namespace Todolist.Dtos
{
    public class AddTaskDto
    {
        public string Description { get; set; }
        public int TaskListId { get; set; }
        public bool IsCompleted { get; set; }
    }
}