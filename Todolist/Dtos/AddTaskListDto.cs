using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Todolist.Dtos
{
    public class AddTaskListDto
    {
        [Required]
        public string Name { get; set; }
        public IdentityUser User { get; set; }
    }
}
