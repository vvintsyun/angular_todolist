using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        
        public virtual DbSet<Tasklists> Tasklists { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }

        public static Context ContextCreate(IHostingEnvironment env)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=%CONTENTROOTPATH%\\todolistdb.mdf;Integrated Security=True; MultipleActiveResultSets=True".Replace("%CONTENTROOTPATH%", env.ContentRootPath));

            return new Context(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tasklists>(entity =>
            {
                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(100);

                //entity.HasOne(d => d.UserNavigation)
                //    .WithMany(p => p.Tasklists)
                //    .HasForeignKey(d => d.User)
                //    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.HasIndex(e => e.TasklistId);

                entity.HasOne(d => d.Tasklist)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TasklistId);
            });
        }
    }
}
