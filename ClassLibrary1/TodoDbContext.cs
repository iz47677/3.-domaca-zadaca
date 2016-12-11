using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EntityFramework
{
    public class TodoDbContext : DbContext
    {
        public IDbSet<TodoItem> TodoItems { get; set; }

        public TodoDbContext(string connectionString) : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(c => c.Id);
            modelBuilder.Entity<TodoItem>().Property(c => c.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.UserId).IsRequired();
        }
    }
}
