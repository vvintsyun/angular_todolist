using System;
using System.Collections.Generic;

namespace Todolist.Models
{
    public partial class Tasklist
    {
        public Tasklist()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }

        //public AspNetUsers UserNavigation { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
