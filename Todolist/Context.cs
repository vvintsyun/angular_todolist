using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Todolist.Models;

namespace Todolist
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=%CONTENTROOTPATH%\\todolistdb.mdf;Integrated Security=True; MultipleActiveResultSets=True".Replace("%CONTENTROOTPATH%", Directory.GetCurrentDirectory()));
        }
        
        public virtual DbSet<Tasklist> Tasklists { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tasklist>(entity =>
            {
                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasIndex(e => e.TasklistId);

                entity.HasOne(d => d.Tasklist)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TasklistId);
            });
        }
    }
}
