namespace Todolist.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int TaskListId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}