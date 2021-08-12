using System.ComponentModel.DataAnnotations;

namespace Todolist.Dtos
{
    public class UpdateTaskListDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}