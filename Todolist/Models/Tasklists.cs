using System;
using System.Collections.Generic;

namespace Todolist.Models
{
    public partial class Tasklists
    {
        public Tasklists()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string User { get; set; }

        //public AspNetUsers UserNavigation { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
    }
}
