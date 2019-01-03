using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todolist.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
           : base(options)
        { }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tasklist> Tasklists { get; set; }


        public static Context ContextCreate(IHostingEnvironment env)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=%CONTENTROOTPATH%\\todolistdb.mdf;Integrated Security=True; MultipleActiveResultSets=True".Replace("%CONTENTROOTPATH%", env.ContentRootPath));

            return new Context(optionsBuilder.Options);
        }
    }
}
