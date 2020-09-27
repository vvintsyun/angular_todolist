namespace Todolist.Dtos
{
    public class AddTaskDto
    {
        public string Description { get; set; }
        public int TasklistId { get; set; }
        public bool Iscompleted { get; set; }
    }
}