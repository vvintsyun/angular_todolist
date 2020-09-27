namespace Todolist.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int TasklistId { get; set; }
        public string Description { get; set; }
        public bool Iscompleted { get; set; }
    }
}