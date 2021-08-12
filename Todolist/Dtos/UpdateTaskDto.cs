﻿using System.ComponentModel.DataAnnotations;

namespace Todolist.Dtos
{
    public class UpdateTaskDto : AddTaskDto
    {
        [Required]
        public int Id { get; set; }
    }
}