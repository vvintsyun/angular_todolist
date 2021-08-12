using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Todolist.Dtos;

namespace Todolist.Models
{
    public class Task
    {
        public int Id { get; protected set; }
        [MaxLength(100)]
        [Required]
        public string Description { get; protected set; }
        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public int TaskListId { get; protected set; }
        public virtual TaskList TaskList { get; protected set; }

        public Task(CreateTaskDto createDto)
        {
            Description = createDto.Description;
            IsCompleted = createDto.IsCompleted;
            TaskList = createDto.TaskList ?? throw new ArgumentException($"{nameof(createDto.TaskList)} is null");
        }
        
        protected Task() {}
    }
}
