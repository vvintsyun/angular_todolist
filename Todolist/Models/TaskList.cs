using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Todolist.Dtos;

namespace Todolist.Models
{
    public class TaskList
    {
        public TaskList(AddTaskListDto createDto)
        {
            Tasks = new HashSet<Task>();

            Name = createDto.Name;
            User = createDto.User ?? throw new ArgumentException($"{nameof(createDto.User)} is null");;
        }
        
        protected TaskList() {}


        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; protected set; }
        
        [Required]
        public string UserId { get; protected set; }
        public IdentityUser User { get; protected set; }
        
        public ICollection<Task> Tasks { get; protected set; }
    }
}
