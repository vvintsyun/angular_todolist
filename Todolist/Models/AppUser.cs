﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todolist.Models
{
    //public class AppUser : IdentityUser
    //{

    //}
    public partial class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
    }
}
