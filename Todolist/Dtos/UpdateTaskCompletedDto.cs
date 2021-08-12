using System.ComponentModel.DataAnnotations;

namespace Todolist.Dtos
{
    public class UpdateTaskCompletedDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
    }
}