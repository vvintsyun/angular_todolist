using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todolist.Models;

namespace Todolist
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=%CONTENTROOTPATH%\\Data\\todolistdb.mdf;Integrated Security=True; MultipleActiveResultSets=True".Replace("%CONTENTROOTPATH%", Directory.GetCurrentDirectory()));
        }
        
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
        
        public virtual DbSet<TaskList> TaskLists { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasIndex(e => e.TaskListId);

                entity.HasOne(d => d.TaskList)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskListId);
            });
        }
    }
}
