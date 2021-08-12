using System.ComponentModel.DataAnnotations;

namespace Todolist.Dtos
{
    public class AddTaskDto
    {
        [MaxLength(100)]
        [Required]
        public string Description { get; set; }
        [Required]
        public int TaskListId { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
    }
}