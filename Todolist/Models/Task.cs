using System;
using System.Collections.Generic;

namespace Todolist.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int TasklistId { get; set; }
        public bool Iscompleted { get; set; }

        public virtual Tasklist Tasklist { get; set; }
    }
}
